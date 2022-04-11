using Concentus.Enums;
using Concentus.Oggfile;
using Concentus.Structs;
using Deepgram;
using Deepgram.Transcription;
using deepgramwinsys.Setting;
using NAudio.Lame;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace deepgramwinsys
{
    public partial class MainForm : Form
    {
        private Task? _ConvertAndTranscriptTask = null;
        private WasapiLoopbackCapture? _WasapiLoopbackCapture = null;
        private CaptionForm? _CaptionForm = null;
        private CancellationTokenSource? _CancellationTokenSource = null;
        private List<DeepGramLanguage> _Languages = new List<DeepGramLanguage>();
        private DeepGramLanguage? _SelectedLanguage;
        private DeepGramModel? _SelectedModel;
        private Settings _Settings;
        /// <summary>
        /// init main form
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            InitValues();
        }
        /// <summary>
        /// Start capture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStartCapture_Click(object sender, EventArgs e)
        {
            //These should never fail but anyway
            _SelectedLanguage = comboBoxLanguage.SelectedItem as DeepGramLanguage;
            if (_SelectedLanguage == null)
            {
                MessageBox.Show("Language not Selected");
            }
            _SelectedModel = comboBoxModel.SelectedItem as DeepGramModel;
            if (_SelectedModel == null)
            {
                MessageBox.Show("Model not Selected");
            }
            _CaptionForm = new CaptionForm();
            if (checkBoxHideCaptionTitle.Checked)
            {
                _CaptionForm.FormBorderStyle = FormBorderStyle.None;
            }
            else
            {
                _CaptionForm.FormBorderStyle = FormBorderStyle.Sizable;
            }
            if (checkBoxTransparentBackground.Checked)
            {
                _CaptionForm.BackColor = Color.Lime;
                _CaptionForm.TransparencyKey = Color.Lime;
            }
            else
            {
                _CaptionForm.BackColor = Color.Black;
            }
            _CaptionForm.Show();
            _CaptionForm.captionLabel.Text = "";
            _CancellationTokenSource = new CancellationTokenSource();
            _ConvertAndTranscriptTask = Task.Factory.StartNew(ConvertAndTranscript, _CancellationTokenSource.Token);
            SetComponentsRecordingEnabled(true);
        }
        /// <summary>
        /// Enable disable capture controls
        /// </summary>
        /// <param name="recording"></param>
        private void SetComponentsRecordingEnabled(bool recording)
        {
            comboBoxLanguage.Enabled = !recording;
            comboBoxModel.Enabled = !recording;
            checkBoxSaveAsCSV.Enabled = !recording;
            checkBoxSaveMP3.Enabled = !recording;
            checkBoxProfinityAllowed.Enabled = !recording;
            buttonStartCapture.Enabled = !recording;
            buttonEndCapture.Enabled = recording;
        }
        /// <summary>
        /// Stop capture function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEndCapture_Click(object sender, EventArgs e)
        {
            if (_CancellationTokenSource != null)
            {
                _WasapiLoopbackCapture?.StopRecording();
                _CancellationTokenSource?.Cancel(false);
                _CaptionForm?.Close();
                _CaptionForm = null;
            }
            SetComponentsRecordingEnabled(false);

        }
        /// <summary>
        /// Convert process and get the text
        /// </summary>
        private async void ConvertAndTranscript()
        {
            //enter credentials for deepgram
            var credentials = new Credentials(textBoxApiKey.Text);
            //Create our export folder to record sound and CSV file
            var outputFolder = CreateRecordingFolder();
            //File settings
            var dateTimeNow = DateTime.Now;
            var fileName = $"{dateTimeNow.Year}_{dateTimeNow.Month}_{dateTimeNow.Day}#{dateTimeNow.Hour}_{dateTimeNow.Minute}_{dateTimeNow.Minute}_record";
            var soundFileName = $"{fileName}.mp3";
            var csvFileName = $"{fileName}.csv";
            var outputSoundFilePath = Path.Combine(outputFolder, soundFileName);
            var outputCSVFilePath = Path.Combine(outputFolder, csvFileName);
            //init deepgram
            var deepgramClient = new DeepgramClient(credentials);
            //init loopback interface
            _WasapiLoopbackCapture = new WasapiLoopbackCapture();
            //generate memory stream and deepgram client
            using (var memoryStream = new MemoryStream())
            using (var deepgramLive = deepgramClient.CreateLiveTranscriptionClient())
            {
                //the format that will we send to deepgram is 24 Khz 16 bit 2 channels  
                var waveFormat = new WaveFormat(24000, 16, 2);
                var deepgramWriter = new WaveFileWriter(memoryStream, waveFormat);
                //mp3 writer if we wanted to save audio
                LameMP3FileWriter? mp3Writer = checkBoxSaveMP3.Checked ?
                    new LameMP3FileWriter(outputSoundFilePath, _WasapiLoopbackCapture.WaveFormat, LAMEPreset.STANDARD_FAST) : null;

                //file writer if we wanted to save as csv
                StreamWriter? csvWriter = checkBoxSaveAsCSV.Checked ? File.CreateText(outputCSVFilePath) : null;
                //deepgram options
                var options = new LiveTranscriptionOptions()
                {
                    Punctuate = true,
                    Diarize = true,
                    Encoding = Deepgram.Common.AudioEncoding.Linear16,
                    ProfanityFilter = checkBoxProfinityAllowed.Checked,
                    Language = _SelectedLanguage.LanguageCode,
                    Model = _SelectedModel.ModelCode,
                };
                //connect 
                await deepgramLive.StartConnectionAsync(options);
                //when we receive data from deepgram this is mostly taken from their samples
                deepgramLive.TranscriptReceived += (s, e) =>
                                 {
                                     try
                                     {
                                         if (e.Transcript.IsFinal &&
                                            e.Transcript.Channel.Alternatives.First().Transcript.Length > 0)
                                         {
                                             var transcript = e.Transcript;
                                             var text = $"{transcript.Channel.Alternatives.First().Transcript}";
                                             _CaptionForm?.captionLabel.BeginInvoke((Action)(() =>
                                             {
                                                 csvWriter?.WriteLine($@"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss \"GMT\"zzz")},""{text}""");
                                                 _CaptionForm.captionLabel.Text = text;
                                                 _CaptionForm?.captionLabel.Refresh();
                                             }));
                                         }
                                     }
                                     catch (Exception ex)
                                     {

                                     }
                                 };
                deepgramLive.ConnectionError += (s, e) =>
                {

                };
                //when windows tell us that there is sound data ready to be processed
                //better than polling
                _WasapiLoopbackCapture.DataAvailable += (s, a) =>
                {
                    mp3Writer?.Write(a.Buffer, 0, a.BytesRecorded);
                    var buffer = ToPCM16(a.Buffer, a.BytesRecorded, _WasapiLoopbackCapture.WaveFormat);
                    deepgramWriter.Write(buffer, 0, buffer.Length);
                    deepgramLive.SendData(memoryStream.ToArray());
                    memoryStream.Position = 0;
                };
                //when recording stopped release and flush all file pointers 
                _WasapiLoopbackCapture.RecordingStopped += (s, a) =>
                {
                    if (mp3Writer != null)
                    {
                        mp3Writer.Dispose();
                        mp3Writer = null;
                    }
                    if (csvWriter != null)
                    {
                        csvWriter.Dispose();
                        csvWriter = null;
                    }
                    _WasapiLoopbackCapture.Dispose();
                };
                _WasapiLoopbackCapture.StartRecording();
                while (_WasapiLoopbackCapture.CaptureState != NAudio.CoreAudioApi.CaptureState.Stopped)
                {
                    if (_CancellationTokenSource?.IsCancellationRequested == true)
                    {
                        _CancellationTokenSource?.Dispose();
                        _CancellationTokenSource = null;
                        return;
                    }
                    Thread.Sleep(500);
                }
            }
        }
        // https://stackoverflow.com/a/65469513
        /// <summary>
        /// Converts an IEEE Floating Point audio buffer into a 16bit PCM compatible buffer.
        /// </summary>
        /// <param name="inputBuffer">The buffer in IEEE Floating Point format.</param>
        /// <param name="length">The number of bytes in the buffer.</param>
        /// <param name="format">The WaveFormat of the buffer.</param>
        /// <returns>A byte array that represents the given buffer converted into PCM format.</returns>
        public byte[] ToPCM16(byte[] inputBuffer, int length, WaveFormat format)
        {
            if (length == 0)
                return new byte[0]; // No bytes recorded, return empty array.

            // Create a WaveStream from the input buffer.
            using var memStream = new MemoryStream(inputBuffer, 0, length);
            using var inputStream = new RawSourceWaveStream(memStream, format);
            {

                // Convert the input stream to a WaveProvider in 16bit PCM format with sample rate of 24000 Hz.
                var convertedPCM = new SampleToWaveProvider16(
                    new WdlResamplingSampleProvider(
                        new WaveToSampleProvider(inputStream),
                        24000)
                    );

                byte[] convertedBuffer = new byte[length];

                using var stream = new MemoryStream();
                int read;

                // Read the converted WaveProvider into a buffer and turn it into a Stream.
                while ((read = convertedPCM.Read(convertedBuffer, 0, length)) > 0)
                    stream.Write(convertedBuffer, 0, read);

                // Return the converted Stream as a byte array.
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Init values
        /// </summary>
        public void InitValues()
        {
            _Settings = AppSettings<Settings>.Load();

            textBoxApiKey.Text = _Settings.ApiKey;
            checkBoxSaveMP3.Checked = _Settings.SaveMP3;
            checkBoxSaveAsCSV.Checked = _Settings.SaveCSV;
            checkBoxProfinityAllowed.Checked = _Settings.ProfinityAllowed;
            checkBoxHideCaptionTitle.Checked = _Settings.CaptionHidden;
            checkBoxTransparentBackground.Checked = _Settings.TransparentBackground;
            if ((_Settings.DeepGramLanguages?.Count ?? 0) == 0)
            {
                InitLanguages();
            }
            else
            {
                _Languages = _Settings.DeepGramLanguages;
            }
            InitComponents();
            comboBoxLanguage.SelectedIndex = _Settings.SelectedLanguageId;
            comboBoxModel.SelectedIndex = _Settings.SelectedModelId;

        }
        //Init languages if settings not found
        public void InitLanguages()
        {
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "en-US: English United States",
                LanguageCode = "en-US",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                    new DeepGramModel{ModelName="meeting",ModelCode="meeting"},
                    new DeepGramModel{ModelName="phonecall",ModelCode="phonecall"},
                    new DeepGramModel{ModelName="voicemail",ModelCode="voicemail"},
                    new DeepGramModel{ModelName="finance",ModelCode="finance"},
                    new DeepGramModel{ModelName="conversationalai",ModelCode="conversationalai"},
                    new DeepGramModel{ModelName="video",ModelCode="video"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "en-AU: English Australia",
                LanguageCode = "en-AU",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "en-GB: English United Kingdom",
                LanguageCode = "en-GB",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "en-IN: English India",
                LanguageCode = "en-IN",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "en-NZ: English New Zealand",
                LanguageCode = "en-NZ",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "zh-CN: China(Simplified Mandarin) beta",
                LanguageCode = "zh-CN",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "zh-TW: Taiwan(Traditional Mandarin) beta",
                LanguageCode = "zh-TW",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "nl: Dutch beta",
                LanguageCode = "nl",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "fr: French",
                LanguageCode = "fr",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "fr-CA: Canada",
                LanguageCode = "fr-CA",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "de: German",
                LanguageCode = "de",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "hi: Hindi",
                LanguageCode = "hi",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "id: Indonesian beta",
                LanguageCode = "en-NZ",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "it: Italian beta",
                LanguageCode = "it",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "ja: Japanese beta",
                LanguageCode = "ja",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "ko: Korean beta",
                LanguageCode = "ko",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "pt: Portuguese",
                LanguageCode = "pt",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "ru: Russian",
                LanguageCode = "ru",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "es: Spanish",
                LanguageCode = "es",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "es-419: Spanish Latin America",
                LanguageCode = "es-419",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "sv: Swedish beta",
                LanguageCode = "sv",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "tr: Turkish",
                LanguageCode = "tr",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
            _Languages.Add(new DeepGramLanguage
            {
                LanguageName = "uk: Ukrainian",
                LanguageCode = "uk",
                Models = new List<DeepGramModel>
                {
                    new DeepGramModel{ModelName="general",ModelCode="general"},
                },
            });
        }
        /// <summary>
        /// Init Component values
        /// </summary>
        public void InitComponents()
        {
            var languageBindingSource = new BindingSource();
            languageBindingSource.DataSource = _Languages;
            comboBoxLanguage.DataSource = languageBindingSource;
            comboBoxLanguage.DisplayMember = "LanguageName";
            comboBoxLanguage.ValueMember = "LanguageCode";

            var selectedLanguage = comboBoxLanguage.SelectedItem as DeepGramLanguage;
            if (selectedLanguage != null)
            {
                var modelBindingSource = new BindingSource();
                modelBindingSource.DataSource = _Languages;
                comboBoxModel.DataSource = selectedLanguage.Models;
                comboBoxModel.DisplayMember = "ModelName";
                comboBoxModel.ValueMember = "ModelCode";
            }
        }
        /// <summary>
        /// Set values on language vlues change
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">parameter</param>

        private void comboBoxLanguage_SelectedValueChanged(object sender, EventArgs e)
        {
            var selectedLanguage = comboBoxLanguage.SelectedItem as DeepGramLanguage;
            if (selectedLanguage != null)
            {
                var modelBindingSource = new BindingSource();
                modelBindingSource.DataSource = _Languages;
                comboBoxModel.DataSource = selectedLanguage.Models;
                comboBoxModel.DisplayMember = "ModelName";
                comboBoxModel.ValueMember = "ModelCode";
            }
        }
        /// <summary>
        /// Close Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }
        /// <summary>
        /// Cleanup and save settings on exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _Settings.SaveMP3 = checkBoxSaveMP3.Checked;
            _Settings.SaveCSV = checkBoxSaveAsCSV.Checked;
            _Settings.ProfinityAllowed = checkBoxProfinityAllowed.Checked;
            _Settings.CaptionHidden = checkBoxHideCaptionTitle.Checked;
            _Settings.TransparentBackground = checkBoxTransparentBackground.Checked;
            _Settings.ApiKey = textBoxApiKey.Text;
            _Settings.SelectedLanguageId = comboBoxLanguage.SelectedIndex;
            _Settings.SelectedModelId = comboBoxModel.SelectedIndex;
            _Settings.DeepGramLanguages = _Languages;
            AppSettings<Settings>.Save(_Settings);
            if (_CancellationTokenSource != null)
            {
                _WasapiLoopbackCapture?.StopRecording();
                _CancellationTokenSource?.Cancel(false);
                _CaptionForm?.Close();
                _CaptionForm = null;
            }
        }

        /// <summary>
        /// Show recordings folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOpenRecordingFolder_Click(object sender, EventArgs e)
        {
            var outputFolder = CreateRecordingFolder();
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = outputFolder,
                UseShellExecute = true,
                Verb = "open"
            });
        }
        /// <summary>
        /// Create folder if ot exists and return folder path
        /// </summary>
        /// <returns></returns>
        private string CreateRecordingFolder()
        {
            var outputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Recorded");
            Directory.CreateDirectory(outputFolder);
            return outputFolder;
        }
        /// <summary>
        /// Hide or show caption title
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxHideCaptionTitle_CheckedChanged(object sender, EventArgs e)
        {
            if (_CaptionForm == null)
            {
                return;
            }
            if ((checkBoxHideCaptionTitle.Checked))
            {
                _CaptionForm.FormBorderStyle = FormBorderStyle.None;
            }
            else
            {
                _CaptionForm.FormBorderStyle = FormBorderStyle.Sizable;
            }
        }
        /// <summary>
        /// Set background transparent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxTransparentBackground_CheckedChanged(object sender, EventArgs e)
        {
            if (_CaptionForm == null)
            {
                return;
            }
            if (checkBoxTransparentBackground.Checked)
            {
                _CaptionForm.BackColor = Color.Lime;
                _CaptionForm.TransparencyKey = Color.Lime;
            }
            else
            {
                _CaptionForm.BackColor = Color.Black;
            }
        }
    }
}