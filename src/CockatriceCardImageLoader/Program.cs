using CockatriceCardImageLoader.Cockatrice;
using CockatriceCardImageLoader.Convertors;
using CockatriceCardImageLoader.Planesculptors;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using System.Runtime.InteropServices;


namespace CockatriceCardImageLoader
{
    internal class Program
    {
        static string DEFAULT_CARD_IMAGE_SERVER = ConfigurationManager.AppSettings["DEFAULT_CARD_IMAGE_SERVER"];
        static string COCKATRICE_DATA_FOLDER = ConfigurationManager.AppSettings["COCKATRICE_DATA_FOLDER"];
        static string COCKATRICE_DEFAULT_CARD_FILENAME = ConfigurationManager.AppSettings["COCKATRICE_DEFAULT_CARD_FILENAME"];
        static string COCKATRICE_DEFAULT_TOKEN_FILENAME = ConfigurationManager.AppSettings["COCKATRICE_DEFAULT_TOKEN_FILENAME"];
        static string COCKATRICE_DEFAULT_CUSTOMSETS_FOLDERNAME = ConfigurationManager.AppSettings["COCKATRICE_DEFAULT_CUSTOMSETS_FOLDERNAME"];
        static string COCKATRICE_DEFAULT_IMAGE_FOLDERNAME = ConfigurationManager.AppSettings["COCKATRICE_DEFAULT_IMAGE_FOLDERNAME"];
        static string COCKATRICE_COSTUM_IMAGE_FOLDERNAME = ConfigurationManager.AppSettings["COCKATRICE_COSTUM_IMAGE_FOLDERNAME"];

        static void Main(string[] args)
        {
            CollectionFile collectionFile = null;
            string collectionFilepath = null;
            string downloadedPicsPath = null;




            log("");
            log("CockatriceCardImageLoader");
            log("");

            if (DEFAULT_CARD_IMAGE_SERVER == null)
            {
                log("Configuration file is missing!\nApplication terminated.");
                return;
            }


            bool done1 = false;
            while (!done1)
            {
                log("Select collection to load:");
                log("1 - Select Cockatrice cards file (cards.xml).");
                log("2 - Select Cockatrice tokens file (tokens.xml).");
                log("3 - Input card collection file manually.");
                log("4 - Convert set from Planesculptors to Cockatrice XML.");
                log("0 - Proceed to operations or to end application.");

                string cmd1 = Console.ReadLine();
                logToFile(cmd1);
                log("");

                try
                {
                    switch (cmd1.Trim().ToLower())
                    {
                        case "0":
                            if (collectionFile != null)
                                done1 = true;
                            else
                                log("You can't proceed to operations until you select a card collection!\n");
                            break;

                        case "1":
                            collectionFilepath = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_CARD_FILENAME);
                            downloadedPicsPath = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_IMAGE_FOLDERNAME);
                            log($"Collection file path: '{collectionFilepath}'");
                            log($"Images output folder: '{downloadedPicsPath}'");
                            log("Load cards file from xml file...");
                            collectionFile = CollectionFile.ImportFromXmlFile(collectionFilepath);
                            log("");
                            printReportForCollectionFile(collectionFile);
                            break;

                        case "2":
                            collectionFilepath = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_TOKEN_FILENAME);
                            downloadedPicsPath = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_IMAGE_FOLDERNAME);
                            log($"Collection file path: '{collectionFilepath}'");
                            log($"Images output folder: '{downloadedPicsPath}'");
                            log("Load tokens file from xml file...");
                            collectionFile = CollectionFile.ImportFromXmlFile(collectionFilepath);
                            log("");
                            printReportForCollectionFile(collectionFile);
                            break;

                        case "3":
                            log("Input collection source file (relative to %cockatrice_data%): ", newLine: false);
                            string cardSourceFile = Console.ReadLine();
                            collectionFilepath = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_CUSTOMSETS_FOLDERNAME, cardSourceFile);
                            downloadedPicsPath = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_COSTUM_IMAGE_FOLDERNAME);
                            log($"Collection file path: '{collectionFilepath}'");
                            log($"Images output folder: '{downloadedPicsPath}'");
                            if (System.IO.File.Exists(collectionFilepath))
                            {
                                collectionFile = CollectionFile.ImportFromXmlFile(collectionFilepath);
                                log("");
                                printReportForCollectionFile(collectionFile);
                            }
                            else
                                log($"File '{collectionFile}' does not existe!");
                            break;

                        case "4":
                            convertPlanesculptorsJsonToCockatriceXml();
                            break;
                    }
                }
                catch(Exception ex)
                {
                    log($"Exception! {ex.Message}", ex);
                    log("");
                }
            }

            bool done2 = false;
            while (!done2)
            {
                log("Select operation:");
                log("11 - Download cards image (English).");
                log("12 - Create set folders.");
                log("13 - Count missing images.");
                log("14 - Name missing images.");
                log("15 - Count missing image defintions.");
                log("16 - Name missing image definitions.");
                log("0 - End application.");

                string cmd2 = Console.ReadLine();
                logToFile(cmd2);
                log("");

                try
                {
                    switch (cmd2.Trim().ToLower())
                    {
                        case "0":
                            done2 = true;
                            break;

                        case "11":
                            downloadCardImages(collectionFile, downloadedPicsPath);
                            break;

                        case "12":
                            createSetFolders(collectionFile, downloadedPicsPath);
                            break;

                        case "13":
                            countMissingImages(collectionFile, downloadedPicsPath);
                            break;

                        case "14":
                            nameMissingImages(collectionFile, downloadedPicsPath);
                            break;

                        case "15":
                            countMissingImageDefintions(collectionFile, downloadedPicsPath);
                            break;

                        case "16":
                            nameMissingImageDefintions(collectionFile, downloadedPicsPath);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    log($"Exception! {ex.Message}", ex);
                    log("");
                }
            }

            log("Application end.");
            Console.ReadKey();
        }



        private static void convertPlanesculptorsJsonToCockatriceXml()
        {
            log("Converting set from Planesculptors to Cockatrice XML...");

            log("Input set code: ", newLine: false);
            string setCode = Console.ReadLine();
            logToFile(setCode);

            log("Input set name: ", newLine: false);
            string setName = Console.ReadLine();
            logToFile(setName);

            log("Input set type (\"Custom[: <game>[:<set>]]\"): ", newLine: false);
            string setType = Console.ReadLine();
            logToFile(setType);

            log("Input set author: ", newLine: false);
            string setAuthor = Console.ReadLine();
            logToFile(setAuthor);

            log("Input set version: ", newLine: false);
            string setVersion = Console.ReadLine();
            logToFile(setVersion);

            log("Input set date: ", newLine: false);
            string setDate = Console.ReadLine();
            logToFile(setDate);

            log("Input PlaneSculptors page url for the set: ", newLine: false);
            string planesculptorsPageUrl = Console.ReadLine();
            logToFile(planesculptorsPageUrl);

            log("Input result file (relative to %cockatrice_data%): ", newLine: false);
            string resultFile = Console.ReadLine();
            logToFile(resultFile);



            log("Converting Planesculptors JSON into Cockatrice XML...");

            resultFile = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_CUSTOMSETS_FOLDERNAME, resultFile);

            string json = SiteInterface.GetSetJson(planesculptorsPageUrl);
            SetCardList cardList = JsonConvert.DeserializeObject<SetCardList>(json);

            CollectionFile cockatriceCollection = PlanesculptorsToCockatriceConverter.Convert(cardList, setCode);

            cockatriceCollection.Sets[0].Name = setCode;
            cockatriceCollection.Sets[0].LongName = setName;
            cockatriceCollection.Sets[0].SetType = setType;
            cockatriceCollection.Sets[0].ReleaseDate = DateTime.Parse(setDate).ToString("yyyy-MM-dd");

            cockatriceCollection.Info.Author = setAuthor;
            cockatriceCollection.Info.SourceVersion = setVersion;
            cockatriceCollection.Info.SourceUrl = planesculptorsPageUrl;
            cockatriceCollection.Info.CreatedAt = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"); 

            cockatriceCollection.ExportToXmlFile(resultFile);

            log("Conversion done.");
            log("");
        }

        private static void printReportForCollectionFile(CollectionFile collectionFile)
        {
            log("Collection loaded from xml file.");

            if (collectionFile.Info != null)
            {
                if (collectionFile.Info.Author != null)
                    log($"Author: {collectionFile.Info.Author}.");

                if (collectionFile.Info.SourceVersion != null)
                    log($"Version: {collectionFile.Info.SourceVersion}.");

            }
            else
            {
                log("Set provide no info!");
            }

            if (collectionFile.Sets != null)
                log($"{collectionFile.Sets.Count().ToString()} set(s).");

            if (collectionFile.Cards != null)
                log($"{collectionFile.Cards.Count().ToString()} cards.");

            log($"{collectionFile.PrintCount.ToString()} prints.");


            log("");
        }

        private static void downloadCardImages(CollectionFile collectionFile, string rootFolder)
        {
            log("Download card images from server...");

            createSetFolders(collectionFile, rootFolder);

            int missingImagesCount = countMissingImages(collectionFile, rootFolder);
            int downloadedPicsCount = 0;

            DateTime start = DateTime.Now;

            foreach (CollectionCard card in collectionFile.Cards)
            {
                foreach (CollectionCardPrint cardPrint in card.Prints)
                {
                    string cardName = fixCardName(card);
                    string cardPrintLocalFilename = Path.Combine(rootFolder, cardPrint.SetCode, cardName + ".jpg");

                    if (!System.IO.File.Exists(cardPrintLocalFilename))
                    {
                        if (cardPrint.HasPrintImage)
                        {
                            log($"Download image for card '{card.Name}' in set '{cardPrint.SetCode}'.");

                            int retries = 5;
                            bool downloaded = downloadCardPrintImage(card, cardPrint, cardPrintLocalFilename);

                            while (!downloaded && retries > 0)
                            {
                                retries--;
                                downloaded = downloadCardPrintImage(card, cardPrint, cardPrintLocalFilename);
                            }

                            if (downloaded)
                                downloadedPicsCount++;

                            // Kindly asked by scryfall between each request.
                            //System.Threading.Thread.Sleep(100);
                        }
                        else
                        {
                            log($"Card '{card.Name}' in set '{cardPrint.SetCode}' does not define an image!");
                        }

                        if (downloadedPicsCount > 0 && downloadedPicsCount % 50 == 0)
                        {
                            log("");
                            log($"{downloadedPicsCount} images downloaded.");

                            DateTime end = DateTime.Now;
                            TimeSpan len = end - start;
                            decimal ticksPerDownload = len.Ticks / downloadedPicsCount;
                            decimal remainingTimeTicks = (missingImagesCount * ticksPerDownload);
                            TimeSpan remainingTimeSpan = new TimeSpan((long)remainingTimeTicks);
                            DateTime timeOfCompletion = DateTime.Now.Add(remainingTimeSpan);

                            log($"Estimated time for completion: {(int)remainingTimeSpan.TotalMinutes} minutes at {timeOfCompletion.ToString()}");
                            log("");
                        }

                    }
                }

            }

            log("Download card images from server terminated.");
            log("");

        }

        private static bool downloadCardPrintImage(CollectionCard card, CollectionCardPrint cardPrint, string cardPrintLocalFilename)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = cardPrint.PrintImageUrl(DEFAULT_CARD_IMAGE_SERVER) + "?" + DateTime.Now.Ticks.ToString();

                    client.DownloadFile(new Uri(url), cardPrintLocalFilename);

                    return true;
                }
                catch (WebException wex)
                {
                    log($"Web Exception! Http Code: {wex.Status} - {wex.Message}", wex);

                    switch ((int)wex.Status)
                    {
                        case 429:
                            // Too Many Requests
                            log("Sleeping for 60 seconds...");
                            System.Threading.Thread.Sleep(60000);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    log($"Exception! {ex.Message}", ex);
                }
            }

            return false;
        }

        private static int countMissingImages(CollectionFile collectionFile, string rootFolder)
        {
            log("Counting missing images...");

            int count = 0;

            foreach (CollectionCard card in collectionFile.Cards)
            {
                foreach (CollectionCardPrint cardPrint in card.Prints)
                {
                    string cardName = fixCardName(card);
                    string cardPrintLocalFilename = Path.Combine(rootFolder, cardPrint.SetCode, cardName + ".jpg");

                    if (!System.IO.File.Exists(cardPrintLocalFilename))
                        count++;
                }
            }

            log($"There are {count} missing images.");
            log("");

            return count;
        }

        private static int nameMissingImages(CollectionFile collectionFile, string rootFolder)
        {
            log("Naming missing images...");

            int count = 0;

            foreach (CollectionCard card in collectionFile.Cards)
            {
                foreach (CollectionCardPrint cardPrint in card.Prints)
                {
                    string cardFixedName = fixCardName(card);
                    string cardPrintLocalFilename = Path.Combine(rootFolder, cardPrint.SetCode, cardFixedName + ".jpg");

                    if (!System.IO.File.Exists(cardPrintLocalFilename))
                    {
                        count++;
                        log($"Image is missing for card '{card.Name}' in set '{cardPrint.SetCode}'.");
                    }
                }
            }

            log($"There are {count} missing images.");
            log("");

            return count;
        }

        private static int countMissingImageDefintions(CollectionFile collectionFile, string rootFolder)
        {
            log("Counting missing image definitions...");

            int count = 0;

            foreach (CollectionCard card in collectionFile.Cards)
                foreach (CollectionCardPrint cardPrint in card.Prints)
                    if (!cardPrint.HasPrintImage)
                        count++;

            log($"There are {count} missing image definitions.");
            log("");

            return count;
        }

        private static int nameMissingImageDefintions(CollectionFile collectionFile, string rootFolder)
        {
            log("Naming missing images definitions...");

            int count = 0;

            foreach (CollectionCard card in collectionFile.Cards)
                foreach (CollectionCardPrint cardPrint in card.Prints)
                    if (!cardPrint.HasPrintImage)
                    {
                        count++;
                        log($"Image definition is missing for card '{card.Name}' in set '{cardPrint.SetCode}'.");
                    }

            log($"There are {count} missing images definitions");
            log("");

            return count;
        }

        private static void checkRootFolderExistence(string rootFolder)
        {
            if (!System.IO.Directory.Exists(rootFolder))
            {
                log($"Create root folder '{rootFolder}'.");
                System.IO.Directory.CreateDirectory(rootFolder);
            }
        }

        private static void createSetFolders(CollectionFile collectionFile, string rootFolder)
        {
            log("Creating set folders...");

            if (collectionFile.Sets == null)
            {
                log("This collection does not contain sets.");
                return;
            }

            checkRootFolderExistence(rootFolder);

            foreach (CollectionSet set in collectionFile.Sets)
            {
                checkSetFolderExistence(rootFolder, set.Name);
            }

            log("Set folders created.");
            log("");

        }

        private static string fixCardName(CollectionCard card)
        {
            string fixedName = card.Name;

            if (fixedName.IndexOfAny(new char[] { '\"', '?' }) >= 0)
            {
                fixedName = fixedName.Replace("\"", "").Replace("?","");
            }
            
            if (fixedName.IndexOfAny(new char[] { '/', '\\' }) >= 0)
            {
                fixedName = fixedName.Replace(" // ", "").Replace("/", "").Replace("\\", "");
            }

            return fixedName;
        }

        private static void checkSetFolderExistence(string rootFolder, string setCode)
        {
            string folderName = Path.Combine(rootFolder, setCode);

            if (!System.IO.Directory.Exists(folderName))
            {
                log($"Create folder for set '{folderName}'.");
                System.IO.Directory.CreateDirectory(folderName);
            }
        }



        private static int logLength = 0;
        
        private static void log(string entry, bool newLine = true)
        {
            logToFile(entry);

            logLength += entry.Length;

            if (logLength > 25000)
            {
                Console.Clear();
                logLength = 0;
            }

            if (newLine)
                Console.WriteLine(entry);
            else
                Console.Write(entry);
        }

        private static void log(string entry, Exception ex)
        {
            log(entry);

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                log(ex.Message);
            }
        }

        private static string logFilepath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), $"operation{DateTime.Now.ToString("yyyyMMdd")}.log");

        private static void logToFile(string entry)
        {
            System.IO.File.AppendAllText(logFilepath, DateTime.Now.ToString() + "\t" + entry + "\r\n");   
        }
    }
}

