using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;
using System.Management;
using System.Media;
using NAudio.Wave;                  // nAudio
using NAudio.Wave.SampleProviders;  // nAudio
using NAudio.CoreAudioApi;
using StoppedEventArgs = NAudio.Wave.StoppedEventArgs;
using OpenAI_API.Chat;
using OpenAI_API;
using System.Net.Http;
using System.Diagnostics.Eventing.Reader;



namespace Transcriber
{
    public partial class MainMenu : MaterialForm
    {
        public static bool eqPanelShown = false; // All Panels Bool var
        public static TimeSpan duration, curPlayTime; // Audio Duration
        public static bool pKilled = false, cmdIsRunning = false, transcriberIsRunning = false, audioIsChosen = false, audioIsPlayed = false;
        public static string file, fileName;
        public static int playedTime = 0, onePercent = 0;
        public static bool mMenuPanelOpen = false, transcriberPanelOpen = false, chatPanelOpen = false, editRecordPanelOpen = false, dbPanelOpen = false, adminPanelOpen = false;
        //public static Process cmd = new Process();
        public static string result;
        // ChatGPT                      <<<<<<  BELOW  >>>>>>
        private string openAIKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
        private string openAIUrl = "https://api.openai.com/v1/chat/completions";
        private System.Windows.Forms.Timer audioPositionTimer; // New method of control audiobarSlider

        private HttpClient client;



        // ChatGPT                      <<<<<<ENDS HERE>>>>>>
        public MainMenu(string username) // String username used to import username from authorization form
        {
            InitializeComponent();
            ShowAudioOptions(false); // Включить/выключить для отображения опции для контроля аудио
            eqAudioPanel.Hide(); // Temporary measure // Do a do {} while cycle with all panels (shown or hided)

            usernameLABEL.Text = username;

            // ChatGPT                      <<<<<<  BELOW  >>>>>>
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", openAIKey);
            // ChatGPT                      <<<<<<ENDS HERE>>>>>>
        
            // MaterialSkin
        
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

        }

        private void ShowHidePanel(string namePanel)
        {
            switch (namePanel)
            {
                case "mMenu":
                    // Closing panels
                    MainMenuPANEL.Show();           //
                    TranscriberPanel.Hide();
                    ChatPanel.Hide();
                    EditRecordPanel.Hide();
                    //dbPanel.Hide();
                    //adminPanel.Hide();

                    //// bool func
                    mMenuPanelOpen = true;          //
                    transcriberPanelOpen = false;
                    chatPanelOpen = false;
                    editRecordPanelOpen = false;
                    dbPanelOpen = false;
                    adminPanelOpen = false;
                    ShowAudioOptions(false);

                    PlayStopAudio("stop");

                    // Small elements to hide
                    eqAudioPanel.Hide();
                    eqPanelShown = false;

                    break;
                case "transcriber":
                    MainMenuPANEL.Hide();
                    TranscriberPanel.Show();        //
                    ChatPanel.Hide();
                    EditRecordPanel.Hide();
                    //dbPanel.Hide();
                    //adminPanel.Hide();
                    //// bool func
                    mMenuPanelOpen = false;
                    transcriberPanelOpen = true;    //
                    chatPanelOpen = false;
                    editRecordPanelOpen = false;
                    dbPanelOpen = false;
                    adminPanelOpen = false;
                    ShowAudioOptions(false);

                    PlayStopAudio("stop");

                    // Small elements to hide
                    eqAudioPanel.Hide();
                    eqPanelShown = false;

                    break;
                case "chat":
                    MainMenuPANEL.Hide();
                    TranscriberPanel.Hide();
                    ChatPanel.Show();               //
                    EditRecordPanel.Hide();
                    //dbPanel.Hide();
                    //adminPanel.Hide();
                    //// bool func
                    mMenuPanelOpen = false;
                    transcriberPanelOpen = false;
                    chatPanelOpen = true;           //
                    editRecordPanelOpen = false;
                    dbPanelOpen = false;
                    adminPanelOpen = false;
                    ShowAudioOptions(false);

                    PlayStopAudio("stop");

                    // Small elements to hide
                    eqAudioPanel.Hide();
                    eqPanelShown = false;

                    break;
                case "edit":
                    MainMenuPANEL.Hide();
                    TranscriberPanel.Hide();
                    ChatPanel.Hide();
                    EditRecordPanel.Show();         //
                    //dbPanel.Hide();
                    //adminPanel.Hide();
                    //// bool func
                    mMenuPanelOpen = false;
                    transcriberPanelOpen = false;
                    chatPanelOpen = false;
                    editRecordPanelOpen = true;     //
                    dbPanelOpen = false;
                    adminPanelOpen = false;
                    ShowAudioOptions(false);

                    PlayStopAudio("stop");

                    // Small elements to hide
                    eqAudioPanel.Hide();
                    eqPanelShown = false;

                    break;
                case "db":
                    MainMenuPANEL.Hide();
                    TranscriberPanel.Hide();
                    ChatPanel.Hide();
                    EditRecordPanel.Hide();
                    //dbPanel.Show();                 //
                    //adminPanel.Hide();
                    //// bool func
                    mMenuPanelOpen = false;
                    transcriberPanelOpen = false;
                    chatPanelOpen = false;
                    editRecordPanelOpen = false;
                    dbPanelOpen = true;             //
                    adminPanelOpen = false;
                    ShowAudioOptions(false);

                    PlayStopAudio("stop");

                    // Small elements to hide
                    eqAudioPanel.Hide();
                    eqPanelShown = false;

                    break;
                case "admin":
                    MainMenuPANEL.Hide();
                    TranscriberPanel.Hide();
                    ChatPanel.Hide();
                    EditRecordPanel.Hide();
                    //dbPanel.Hide();
                    //adminPanel.Show();              //
                    //// bool func
                    mMenuPanelOpen = false;
                    transcriberPanelOpen = false;
                    chatPanelOpen = false;
                    editRecordPanelOpen = false;
                    dbPanelOpen = false;
                    adminPanelOpen = true;         //
                    ShowAudioOptions(false);

                    PlayStopAudio("stop");

                    // Small elements to hide
                    eqAudioPanel.Hide();
                    eqPanelShown = false;

                    break;
            }
        }

        // nAudio Code                  <<<<<<  BELOW  >>>>>>
        private WaveOutEvent outputDevice;  // nAudio
        private AudioFileReader audioFile;  // nAudio

        string infile = file, outfile, stereoFilePath, monoFilePath; // Входной файл // Путь к нему // CONVERT // was "var"
        //string outfile = file.Replace(".mp3", ".wav"); // Выходной файл // Путь к нему // CONVERT
        //string stereoFilePath = file.Replace(".wav", "_stereo.wav"); // Выходной файл stereo // Путь к нему // CONVERT
        //string monoFilePath = file.Replace(".wav", "_mono.wav"); // Выходной файл mono // Путь к нему // CONVERT
        public DateTime someDate;
        private void ReplacementForEveryType() // !!!USE ONLY FOR COPY-PASTE !!!
        {
            string outfile = file.Replace(".mp3", ".wav"); // Выходной файл // Путь к нему // CONVERT
            string stereoFilePath = file.Replace(".wav", "_stereo.wav"); // Выходной файл stereo // Путь к нему // CONVERT
            string monoFilePath = file.Replace(".wav", "_mono.wav"); // Выходной файл mono // Путь к нему // CONVERT
        }
        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            outputDevice.Dispose();
            outputDevice = null;
            audioFile.Dispose();
            audioFile = null;
        }
        private void RewindResume()
        {
            var closing = false;
            outputDevice.PlaybackStopped += (s, a) => { if (closing) { outputDevice.Dispose(); audioFile.Dispose(); } };
            outputDevice.Init(audioFile);
            this.FormClosing += (s, a) => { closing = true; outputDevice.Stop(); };
            this.ShowDialog();
        }
        private void AudioConverter()
        {
            using (var reader = new MediaFoundationReader(infile))
            {
                WaveFileWriter.CreateWaveFile(outfile, reader);
            }
        }
        public static void AudioTrimmer(string inPath, string outPath, TimeSpan cutFromStart, TimeSpan cutFromEnd)
        {
            using (WaveFileReader reader = new WaveFileReader(inPath))
            {
                using (WaveFileWriter writer = new WaveFileWriter(outPath, reader.WaveFormat))
                {
                    int bytesPerMillisecond = reader.WaveFormat.AverageBytesPerSecond / 1000;

                    int startPos = (int)cutFromStart.TotalMilliseconds * bytesPerMillisecond;
                    startPos = startPos - startPos % reader.WaveFormat.BlockAlign;

                    int endBytes = (int)cutFromEnd.TotalMilliseconds * bytesPerMillisecond;
                    endBytes = endBytes - endBytes % reader.WaveFormat.BlockAlign;
                    int endPos = (int)reader.Length - endBytes;

                    AudioTrimmer(reader, writer, startPos, endPos);
                }
            }
        }
        private static void AudioTrimmer(WaveFileReader reader, WaveFileWriter writer, int startPos, int endPos) // Cut Audio
        {
            reader.Position = startPos;
            byte[] buffer = new byte[1024];
            while (reader.Position < endPos)
            {
                int bytesRequired = (int)(endPos - reader.Position);
                if (bytesRequired > 0)
                {
                    int bytesToRead = Math.Min(bytesRequired, buffer.Length);
                    int bytesRead = reader.Read(buffer, 0, bytesToRead);
                    if (bytesRead > 0)
                    {
                        writer.WriteData(buffer, 0, bytesRead);
                    }
                }
            }
        }
        private void MonoToStereo() // Convert Mono to Stereo
        {
            using (var inputReader = new AudioFileReader(file))
            {
                // convert mono ISampleProvider to stereo
                var stereo = new MonoToStereoSampleProvider(inputReader);
                stereo.LeftVolume = 0.0f; // Silence in left channel
                stereo.RightVolume = 1.0f; // full volume in right channel

                // can either use this for playback:
                outputDevice.Init(stereo);
                outputDevice.Play();

                // write the stereo audio out to a WAV file
                WaveFileWriter.CreateWaveFile16(stereoFilePath, stereo);
            }
        }
        private void StereoToMono() // Convert Stereo to Mono
        {
            using (var inputReader = new AudioFileReader(stereoFilePath))
            {
                // convert stereo ISampleProvider to mono
                var mono = new StereoToMonoSampleProvider(inputReader);
                mono.LeftVolume = 0.0f; // discard the left channel // В будущем выбирать какой канал оставить, либо вообще оставить оба
                mono.RightVolume = 1.0f; // keep the right channel // для этого просто менять значения "0.0f" до "1.0f" и обратно

                // can either use this for playback:
                outputDevice.Init(mono);
                outputDevice.Play();

                // write the stereo audio out to a WAV file
                WaveFileWriter.CreateWaveFile16(monoFilePath, mono);
            }
        }
        private void MixTwoAudio() // Mixing audio files together
        {
            using (var reader1 = new AudioFileReader("file1.wav"))
            using (var reader2 = new AudioFileReader("file2.wav"))
            {
                reader1.Volume = 0.75f;
                reader2.Volume = 0.75f;

                var mixer = new MixingSampleProvider(new[] { reader1, reader2 });
                WaveFileWriter.CreateWaveFile16("mixed.wav", mixer);
            }
        }
        private void UniteAudio() // Unite 2+ audio files (Requires equal Resample)
        {
            var inputFiles = new List<string>
            {
                "first.wav",
                "second.wav",
                "third.wav"
                // Добавьте здесь пути к другим аудиофайлам, которые вы хотите объединить
            };

            var outputFileName = "concatenated.wav";

            var audioReaders = new List<AudioFileReader>();

            // Загрузка и чтение всех указанных аудиофайлов
            foreach (var fileName in inputFiles)
            {
                var audioReader = new AudioFileReader(fileName);
                audioReaders.Add(audioReader);
            }

            // Настройка MixingSampleProvider для объединения потоков
            var mixer = new MixingSampleProvider(audioReaders.Select(reader => reader.ToSampleProvider()));

            // Создание выходного WAV-файла для объединенной аудиозаписи
            WaveFileWriter.CreateWaveFile16(outputFileName, mixer);

            // Освобождение ресурсов
            foreach (var audioReader in audioReaders)
            {
                audioReader.Dispose();
            }
        }
        private void ResampleAudio() // Меняет герцовку аудио, для дальнейшей работы с файлом. Если не сделать этого, то разные по герцовке файлы не получится соединить
        {
            int outRate = 16000;
            var inFile = @"test.mp3";
            var outFile = @"test_resampled.wav";

            using (var reader = new AudioFileReader(inFile))
            {
                // Создаем объект для изменения частоты дискретизации
                var resampler = new WaveFormatConversionStream(new WaveFormat(outRate, reader.WaveFormat.BitsPerSample, reader.WaveFormat.Channels), reader);

                // Создаем выходной WAV-файл с новой частотой дискретизации
                WaveFileWriter.CreateWaveFile(outFile, resampler);
            }
        }
        private void BoostAudio()
        {
            BoostAudioBTN.Enabled = false;
            // Загрузка файла с помощью NAudio
            using (var reader = new AudioFileReader(file))
            {
                // Создание нового .wav файла с примененными фильтрами
                using (var output = new WaveFileWriter(file.Replace(".wav", "_boosted.wav"), reader.WaveFormat))
                {
                    // Коэффициент усиления средних частот (измените по вашему усмотрению)
                    float boostFactor = 6.0f; // На 2 работает лучше всего

                    // Буфер для обработки аудио
                    var buffer = new float[reader.WaveFormat.SampleRate * reader.WaveFormat.Channels];

                    int bytesRead;
                    while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // Умножаем амплитуду аудиосэмплов на коэффициент усиления
                        for (int i = 0; i < bytesRead; i++)
                        {
                            buffer[i] *= boostFactor;
                        }

                        // Записываем обработанные данные в выходной файл
                        output.WriteSamples(buffer, 0, bytesRead);
                    }
                }
            }
            BoostAudioBTN.Enabled = true;
        }
        // nAudio Code                  <<<<<<ENDS HERE>>>>>>

        // GPT Code                     <<<<<<BELOW>>>>>>
        private async Task<string> GetChatBotResponse(string userInput)
        {
            var requestData = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = "You are a text editor." // was helpful assistant
                    },
                    new
                    {
                        role = "user",
                        content = userInput
                    }
                }
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(openAIUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }
        private async void SendButton_Click(object sender, EventArgs e)
        {
            var userInput = userInputTextBox.Text;
            var chatBotResponse = await GetChatBotResponse(userInput);

            // Обработка ответа чат-бота
            var answer = ProcessChatBotResponse(chatBotResponse);

            // Разделение ответа на строки
            var responseLines = answer.Split('\n');

            // Отображение ответа в ListBox
            ChatDisplayListBox.Items.Add(new MaterialListBoxItem { Text = $"User: {userInput}" });

            foreach (var line in responseLines)
            {
                ChatDisplayListBox.Items.Add(new MaterialListBoxItem { Text = $"ChatBot: {line}" });
            }

            ChatDisplayListBox.Items.Add(new MaterialListBoxItem { Text = "-----------------------------" });

            // Очистка поля ввода
            userInputTextBox.Text = string.Empty;
            // Запись ответа в файл
            WriteResponseToFile(answer);
            Console.WriteLine(chatBotResponse);
        }
        private string ProcessChatBotResponse(string response)
        {
            try
            {
                // Разбор JSON-ответа
                dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(response);

                // Извлечение текста сообщения из JSON
                string chatBotMessage = jsonObject.choices[0].message.content;

                return chatBotMessage;
            }
            catch (Exception ex)
            {
                // Обработка возможных ошибок при разборе JSON
                return "Error processing response";
            }
        }
        private void WriteResponseToFile(string response)
        {
            string filePath = "chat_responses.txt"; // указываете путь к файлу

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(response);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing to file: {ex.Message}");
            }
        }
        // GPT Code                     <<<<<<ENDS HERE>>>>>>

        private static void CmdToText(Process cmd) // попробовать всунуть эту функцию в timer_tick, при параметрах transcriberIsRunning
        {
            using (StreamReader reader = cmd.StandardOutput)
            {
                result = reader.ReadToEnd(); // считываем вывод консоли
            }
        }

        private void PlayStopAudio(string type)
        {
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            //player.SoundLocation = file;
            //player.Play();
            if (type == "play")
            {
                pauseMusicBTN.Visible = true;
                playMusicBTN.Visible = false;

                AudioTimer.Start();

                if (outputDevice == null) // nAUDIO
                {
                    outputDevice = new WaveOutEvent();
                    outputDevice.PlaybackStopped += OnPlaybackStopped;
                }
                if (audioFile == null)
                {
                    audioFile = new AudioFileReader(file); // OpenFileDialog Result
                    outputDevice.Init(audioFile);
                }
                duration = audioFile.TotalTime;
                outputDevice.Play();
                audioIsPlayed = true;
            }
            if (type == "pause")
            {
                pauseMusicBTN.Visible = false;
                playMusicBTN.Visible = true;

                AudioTimer.Stop();

                outputDevice?.Pause(); // nAUDIO
                audioIsPlayed = false;
            }
            if (type == "stop")
            {
                pauseMusicBTN.Visible = false;
                playMusicBTN.Visible = true;

                AudioTimer.Stop();
                audioBarSlider.Value = 0;

                outputDevice?.Stop(); // nAUDIO

                audioIsPlayed = false;

                audioIsChosen = false;
                AudioNameTextBox.Text = string.Empty;
                timeRemainingLBL.Text = string.Empty;
            }
        }
        private void timer1_Tick(object sender, EventArgs e /*, string mode*/)
        {
            if (transcriberIsRunning && File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{file.Replace(".wav", ".txt")}")))
            {
                TranscribedAudioTextBox.Text = $"audioIsChosen={audioIsChosen} \n transcriberIsRunning={transcriberIsRunning} \n pKilled={pKilled} \n cmdIsRunning={cmdIsRunning} \n ";
                Console.WriteLine("Файл найден в папке программы!");   // Лютый костыль..ИПОЛЬЗУЕМ!!!!
                KillProcessCmd("whisper.exe");
                KillProcessCmd("cmd.exe");
                cmdIsRunning = false;
            }
            if (audioIsChosen && (audioFile != null && file!= string.Empty))
            {
                TimeSpan curPlayTime = TimeSpan.FromSeconds(audioFile.Position / (double)audioFile.WaveFormat.AverageBytesPerSecond);
                TimeSpan duration = TimeSpan.FromSeconds(audioFile.Length / (double)audioFile.WaveFormat.AverageBytesPerSecond);
                AudioNameTextBox.Text = fileName;
                timeRemainingLBL.Text = $"{curPlayTime.ToString(@"hh\:mm\:ss")} / {duration.ToString(@"hh\:mm\:ss")}";
            }
            if (audioIsPlayed && (audioFile != null && file != string.Empty))
            {
                //onePercent = (int)duration.TotalSeconds / 100; // 507,456
                //curPlayTime = TimeSpan.FromMilliseconds(outputDevice.GetPosition() * 1000.0 / outputDevice.OutputWaveFormat.BitsPerSample / outputDevice.OutputWaveFormat.Channels * 8.0 / outputDevice.OutputWaveFormat.SampleRate);
                TimeSpan curPlayTime = TimeSpan.FromSeconds(audioFile.Position / (double)audioFile.WaveFormat.AverageBytesPerSecond);
                TimeSpan duration = TimeSpan.FromSeconds(audioFile.Length / (double)audioFile.WaveFormat.AverageBytesPerSecond);
                //if ((curPlayTime.TotalSeconds) >= onePercent) audioBarSlider.Value = (int) curPlayTime.TotalSeconds / onePercent;
                timeRemainingLBL.Text = $"{curPlayTime.ToString(@"hh\:mm\:ss")} / {duration.ToString(@"hh\:mm\:ss")}";
                playedTime++;
                
                // DEBUG TEXTBOX //
                //TranscribedAudioTextBox.Text = $"outputPosition = {outputDevice.GetPosition()} \n value = {audioBarSlider.Value} \n curPlayTime = {curPlayTime.TotalSeconds} \n audioBarSlider.ValueMax = {audioBarSlider.ValueMax} \n onePercent = {onePercent}";                
            }
        }
        private void ShowAudioOptions(bool show)
        {
            if (show)
            {
                audioRedactBTN.Enabled = true;
                audioBarSlider.Enabled = true;
                playMusicBTN.Enabled = true;
                audioVolumeTrackBar.Enabled = true;
                TranscribeButtonPanel.Show();
            }
            else if (!show)
            {
                audioRedactBTN.Enabled = false;
                audioBarSlider.Enabled = false;
                playMusicBTN.Enabled = false;
                audioVolumeTrackBar.Enabled = false;
                TranscribeButtonPanel.Hide();
            }
        }
        private void OpenFile()
        {
            // WORKING CODE BELOW <<<<<
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "C:\\Users\\DedMo\\source\\repos\\Transcriber\\bin\\Debug\\Audio\\";
            ofd.Filter = "WAV files (*.wav)|*.wav";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = Path.GetFileNameWithoutExtension(ofd.FileName);
                file = ofd.FileName;
                audioIsChosen = true;
                //audioBarSlider.ValueMax = (int) duration.TotalMilliseconds / 100; // Максимальное значение слайдера = 100%
                audioBarSlider.Value = 0;
                ShowAudioOptions(true);
            }

            string languageDetected = LanguageDetector.DetectLanguage(fileName);

            if (languageDetected == "Russian")
            {
                string oldfile = file;
                string newFileName = TransliterationHelper.Transliterate(fileName);

                string destinationDirectory = Path.GetDirectoryName(file);
                string newFilePath = Path.Combine(destinationDirectory, newFileName + ".wav");

                System.IO.File.Move(file, newFilePath);
                file = newFilePath;
                fileName = newFileName;
            }
            // ENDS HERE >>>>>>

        }
        private void MainMenuBTN_Click(object sender, EventArgs e)
        {
            ShowHidePanel("mMenu");
        }

        private void EditRecordBTN_Click(object sender, EventArgs e)
        {
            ShowHidePanel("edit");
        }

        private void AiAudioBTN_Click(object sender, EventArgs e)
        {
            // Прописать функцию обработки .txt документа нейросеткой


        }

        public async void BulkExecution()
        {
            await Task.Run(() => TranscriberCmd(file, "cmdLine"));
            TranscribedAudioTextBox.Text = result;
        }
        private void ChatBotBTN_Click(object sender, EventArgs e)
        {
            ShowHidePanel("chat");
        }
        private static void KillProcessCmd(string cmdLine)
        {
            pKilled = true;
            Process cmd1 = new Process();
            cmd1.StartInfo.FileName = "cmd.exe";
            cmd1.StartInfo.UseShellExecute = false;
            cmd1.StartInfo.RedirectStandardOutput = true;
            cmd1.StartInfo.RedirectStandardInput = true;
            cmd1.StartInfo.CreateNoWindow = true;
            cmd1.Start();
            //Console.WriteLine("Начало убийства процесса");
            cmd1.StandardInput.WriteLine($"taskkill /f /IM {cmdLine}");
            cmd1.StandardInput.Flush();
            cmd1.StandardInput.Close();
            cmd1.WaitForExit();
            pKilled = false;
        }

        private void BoostAudioBTN_Click(object sender, EventArgs e)
        {
            BoostAudio();
        }

        private void StartTranscribeBTN_Click(object sender, EventArgs e)
        {
            //BulkExecution(); // Starts Transcribation
            TranscribationAlreadyExist(file);
        }

        private void TranscribationAlreadyExist(string file) // 06.06.23 Здеся остановился
        {
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{file.Replace(".wav", ".txt")}")))
            {
                Console.WriteLine("Файл найден в папке программы!");   // Лютый костыль..ИПОЛЬЗУЕМ!!!!
                MessageBox.Show("Транскрибированный файл уже существует!");
            }
            else if (!transcriberIsRunning && !cmdIsRunning && audioIsChosen)
            {
                BulkExecution();
                //TranscriberCmd(fileName, "cmdLine");
            }
        }

        private void audioBarSlider_onValueChanged(object sender, int newValue)
        {
            if (audioFile != null)
            {
                double newPosition = (double)newValue / audioBarSlider.ValueMax;
                audioFile.Position = (long)(newPosition * audioFile.Length);
            }
            //if (audioFile != null)
            //{
            //    audioFile.Position = (long)(audioBarSlider.Value * audioFile.WaveFormat.AverageBytesPerSecond);
            //}
        }

        private void audioVolumeTrackBar_Scroll(object sender, EventArgs e)
        {
            audioVolumeTrackBar.Scroll += (s, a) => audioFile.Volume = audioVolumeTrackBar.Value / 100f;  // "af" - AudioFileReader
        }

        private static void TranscriberCmd(string fileName, string cmdLine) // На данный момент работает. Не записывает лог cmd. Не утилизирует оперативную память после функции. 
        {
            transcriberIsRunning = true;
            cmdIsRunning = true;
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.CreateNoWindow = true;
            //cmd.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);
            cmd.Start();
            cmd.StandardInput.WriteLine("cd audio");
            cmd.StandardInput.WriteLine($"whisper \"{fileName}\"");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            CmdToText(cmd);
            cmd.WaitForExit();
            transcriberIsRunning = false;
        }
        private void TranscriberBTN_Click(object sender, EventArgs e)
        {
            ShowHidePanel("transcriber");
        }
        private void AudioTimer_Tick(object sender, EventArgs e)
        {
            if (audioFile != null)
            {
                double currentPosition = (double)audioFile.Position / audioFile.Length;
                audioBarSlider.Value = (int)(currentPosition * audioBarSlider.ValueMax);
            }
        }
        private void pauseMusicBTN_Click(object sender, EventArgs e)
        {
            PlayStopAudio("pause");
        }

        private void audioRedactBTN_Click(object sender, EventArgs e)
        {
            if (eqPanelShown)
            {
                eqAudioPanel.Hide();
                eqPanelShown = false;
            }
            else if (!eqPanelShown)
            {
                eqAudioPanel.Show();
                eqPanelShown = true;
            }
        }

        private void FileChooseBTN_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void playMusicBTN_Click(object sender, EventArgs e)
        {
            PlayStopAudio("play"); // Play// Pause// Stop

            // New method

            audioPositionTimer = new System.Windows.Forms.Timer();
            audioPositionTimer.Interval = 100; // Интервал обновления в миллисекундах (можно изменить по вашему усмотрению)
            audioPositionTimer.Tick += new EventHandler(AudioTimer_Tick);
            audioPositionTimer.Start();
        }
    }
}