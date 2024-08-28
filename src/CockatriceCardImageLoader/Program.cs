using CockatriceCardImageLoader.Cockatrice;
using CockatriceCardImageLoader.Convertors;
using CockatriceCardImageLoader.DeepL;
using CockatriceCardImageLoader.Google;
using CockatriceCardImageLoader.Planesculptors;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Net;
using System.Reflection.Metadata.Ecma335;
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
        static string COCKATRICE_DEFAULT_DECKS_FOLDERNAME = ConfigurationManager.AppSettings["COCKATRICE_DEFAULT_DECKS_FOLDERNAME"];
        static string COCKATRICE_DEFAULT_IMAGE_FOLDERNAME = ConfigurationManager.AppSettings["COCKATRICE_DEFAULT_IMAGE_FOLDERNAME"];
        static string COCKATRICE_COSTUM_IMAGE_FOLDERNAME = ConfigurationManager.AppSettings["COCKATRICE_COSTUM_IMAGE_FOLDERNAME"];

        private static Dictionary<string, string> translationBuffer;
        private static Dictionary<string, int> nameBuffer;


        static void Main(string[] args)
        {
            CollectionFile collectionFile = null;
            string collectionFilepath = null;
            string downloadedPicsPath = null;

            Logger.Log("");
            Logger.Log("CockatriceCardImageLoader");
            Logger.Log("");

            if (DEFAULT_CARD_IMAGE_SERVER == null)
            {
                Logger.Log("Configuration file is missing!\nApplication terminated.");
                return;
            }

            bool done3 = false;
            while (!done3)
            {


                bool done1 = false;
                while (!done1)
                {
                    Logger.Log("Select collection to load:");
                    Logger.Log("1 - Select Cockatrice cards file (cards.xml).");
                    Logger.Log("2 - Select Cockatrice tokens file (tokens.xml).");
                    Logger.Log("3 - Input card collection file manually.");
                    Logger.Log("4 - Convert set from Planesculptors to Cockatrice collection.");
                    Logger.Log("5 - Batch convert set from Planesculptors to Cockatrice collection.");
                    Logger.Log("6 - Create a new Cockatrice collection.");
                    Logger.Log("0 - Proceed to operations or to end application.");

                    string cmd1 = Console.ReadLine();
                    Logger.LogToFile(cmd1);
                    Logger.Log("");

                    try
                    {
                        switch (cmd1.Trim().ToLower())
                        {
                            case "0":
                                if (collectionFile != null)
                                    done1 = true;
                                else
                                    Logger.Log("You can't proceed to operations until you select a card collection!\n");
                                break;

                            case "1":
                                collectionFilepath = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_CARD_FILENAME);
                                downloadedPicsPath = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_IMAGE_FOLDERNAME);
                                Logger.Log($"Collection file path: '{collectionFilepath}'");
                                Logger.Log($"Images output folder: '{downloadedPicsPath}'");
                                Logger.Log("Load cards file from xml file...");
                                collectionFile = CollectionFile.ImportFromXmlFile(collectionFilepath);
                                Logger.Log("");
                                printReportForCollectionFile(collectionFile);
                                break;

                            case "2":
                                collectionFilepath = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_TOKEN_FILENAME);
                                downloadedPicsPath = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_IMAGE_FOLDERNAME);
                                Logger.Log($"Collection file path: '{collectionFilepath}'");
                                Logger.Log($"Images output folder: '{downloadedPicsPath}'");
                                Logger.Log("Load tokens file from xml file...");
                                collectionFile = CollectionFile.ImportFromXmlFile(collectionFilepath);
                                Logger.Log("");
                                printReportForCollectionFile(collectionFile);
                                break;

                            case "3":
                                Logger.Log("Input collection source file (relative to %cockatrice_data%\\customsets): ", newLine: false);
                                string cardSourceFile = Console.ReadLine();
                                collectionFilepath = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_CUSTOMSETS_FOLDERNAME, cardSourceFile);
                                downloadedPicsPath = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_COSTUM_IMAGE_FOLDERNAME);
                                Logger.Log($"Collection file path: '{collectionFilepath}'");
                                Logger.Log($"Images output folder: '{downloadedPicsPath}'");
                                if (System.IO.File.Exists(collectionFilepath))
                                {
                                    collectionFile = CollectionFile.ImportFromXmlFile(collectionFilepath);
                                    Logger.Log("");
                                    printReportForCollectionFile(collectionFile);
                                }
                                else
                                    Logger.Log($"File '{collectionFile}' does not existe!");
                                break;

                            case "4":
                                convertPlanesculptorsJsonToCockatriceXml();
                                break;

                            case "5":
                                batchConvertPlanesculptorsJsonToCockatriceXml();
                                break;

                            case "6":
                                createNewCockatriceCollection();
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Exception! {ex.Message}", ex);
                        Logger.Log("");
                    }
                }

                bool done2 = false;
                while (!done2)
                {
                    Logger.Log("Select operation:");
                    Logger.Log("11 - Download cards image (English).");
                    Logger.Log("12 - Create set folders.");
                    Logger.Log("13 - Count missing images.");
                    Logger.Log("14 - Name missing images.");
                    Logger.Log("15 - Count missing image defintions.");
                    Logger.Log("16 - Name missing image definitions.");
                    Logger.Log("17 - Count titles and texts total length.");
                    Logger.Log("18 - Add deck from selected collection to another collection.");
                    Logger.Log("19 - Normalize token names.");
                    Logger.Log("20 - Translate collection.");
                    Logger.Log("0 - Previous menu.");
                    Logger.Log("100 - End application.");

                    string cmd2 = Console.ReadLine();
                    Logger.LogToFile(cmd2);
                    Logger.Log("");

                    try
                    {
                        switch (cmd2.Trim().ToLower())
                        {
                            case "100":
                                done3 = true;
                                done2 = true;
                                break;

                            case "0":
                                done3 = false;
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

                            case "17":
                                countCharacters(collectionFile);
                                break;

                            case "18":
                                addDeckToAnotherCollection(collectionFile);
                                break;

                            case "19":
                                normalizeTokenNames(collectionFile);
                                break;

                            case "20":
                                translateCollection(collectionFile);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Exception! {ex.Message}", ex);
                        Logger.Log("");
                    }
                }
            }

            Logger.Log("Application end.");
            Console.ReadKey();
        }

        private static void normalizeTokenNames(CollectionFile collectionFile)
        {
            Logger.Log("Normalizing token names...");

            Logger.Log("Input result file (relative to %cockatrice_data%/customsets): ", newLine: false);
            string resultFile = Console.ReadLine();
            Logger.LogToFile(resultFile);

            resultFile = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_CUSTOMSETS_FOLDERNAME, resultFile);

            nameBuffer = new Dictionary<string, int>();

            if (collectionFile.Cards != null)
                foreach (CollectionCard card in collectionFile.Cards)
                {
                    string trimmedName = card.Name.Trim();

                    trimmedName = removeTokenKeywords(trimmedName);

                    // handle trailling spaces
                    if (!nameBuffer.ContainsKey(trimmedName))
                    {
                        nameBuffer.Add(trimmedName, 1);
                        card.Name = trimmedName;
                    }
                    else
                    {
                        nameBuffer[trimmedName]++;
                        card.Name = trimmedName + $" ({nameBuffer[trimmedName]})";
                    }

                    card.Properties.MainType = normalizeTokenKeywords(card.Properties.MainType);
                    card.Properties.Type = normalizeTokenKeywords(card.Properties.Type);
                }

            collectionFile.ExportToXmlFile(resultFile);

            Logger.Log("Normalizition done.");
            Logger.Log("");
        }

        private static string normalizeTokenKeywords(string name)
        {
            if (name == null)
                return name;

            name = normalizeTokenKeyword(name, "Token");
            name = normalizeTokenKeyword(name, "Emblem");
            name = normalizeTokenKeyword(name, "Enchanment");
            name = normalizeTokenKeyword(name, "Dungeon");

            return name;
        }

        private static string removeTokenKeywords(string name)
        {
            if (name == null)
                return name;

            name = removeTokenKeyword(name, "Token");
            name = removeTokenKeyword(name, "Emblem");
            name = removeTokenKeyword(name, "Enchanment");
            name = removeTokenKeyword(name, "Dungeon");

            return name;
        }

        private static string removeTokenKeyword(string name, string keyword)
        {
            // Token Artifact Creature — Construct
            // Token - Artifact Creature — Construct
            // (Token) Artifact Creature — Construct

            // Artifact Creature — Construct Token
            // Artifact Creature — Construct - Token
            // Artifact Creature — Construct (Token)

            if (name.StartsWith($"{keyword} - "))
                name = name.Substring(keyword.Length + 3);

            if (name.StartsWith($"{keyword} "))
                name = name.Substring(keyword.Length + 1);

            if (name.StartsWith($"({keyword}) "))
                name = name.Substring(keyword.Length + 3);


            if (name.EndsWith($" - {keyword}"))
                name = name.Substring(0, name.Length - keyword.Length - 3);

            if (name.EndsWith($" {keyword}"))
                name = name.Substring(0, name.Length - keyword.Length - 1);

            if (name.EndsWith($" ({keyword})"))
                name = name.Substring(0, name.Length - keyword.Length - 3);

            return name;
        }

        private static string normalizeTokenKeyword(string name, string keyword)
        {
            // should be: Token - Artifact Creature - Construct


            // Token - Artifact Creature - Construct
            // Token Artifact Creature - Construct
            // (Token) Artifact Creature - Construct

            if (name.StartsWith($"{keyword} - "))
                name = $"{keyword} - " + name.Substring(keyword.Length + 3);

            else if (name.StartsWith($"{keyword} "))
                name = $"{keyword} - " + name.Substring(keyword.Length + 1);

            else if (name.StartsWith($"({keyword}) "))
                name = $"{keyword} - " + name.Substring(keyword.Length + 3);


            // Artifact Creature - Construct Token
            // Artifact Creature - Construct - Token
            // Artifact Creature - Construct (Token)

            if (name.EndsWith($" - {keyword}"))
                name = $"{keyword} - " + name.Substring(0, name.Length - keyword.Length - 3);

            else if (name.EndsWith($" {keyword}"))
                name = $"{keyword} - " + name.Substring(0, name.Length - keyword.Length - 1);

            else if (name.EndsWith($" ({keyword})"))
                name = $"{keyword} - " + name.Substring(0, name.Length - keyword.Length - 3);


            // Artifact Creature Token - Construct
            // Artifact Creature - Token - Construct
            // Artifact Creature (Token) - Construct
            // Artifact Creature (Token) Construct
            // Artifact Creature Token Construct

            if (name.IndexOf($" - {keyword} - ") >= 0)
                name = $"{keyword} - " + name.Replace($" - {keyword} - ", " - ");

            else if (name.IndexOf($" {keyword} - ")>=0)
                name = $"{keyword} - " + name.Replace($" {keyword} - "," - ");

            else if (name.IndexOf($" ({keyword}) - ") >= 0)
                name = $"{keyword} - " + name.Replace($" ({keyword}) - ", " - ");

            else if (name.IndexOf($" ({keyword}) ") >= 0)
                name = $"{keyword} - " + name.Replace($" ({keyword}) ", " ");

            else if (name.IndexOf($" {keyword} ") >= 0)
                name = $"{keyword} - " + name.Replace($" {keyword} ", " ");

            return name;
        }

        private static void translateCollection(CollectionFile collectionFile)
        {
            Logger.Log("Translating Cokactrice collection...");

            Logger.Log("Input source language: ", newLine: false);
            string sourceLanguage = Console.ReadLine();
            Logger.LogToFile(sourceLanguage);

            Logger.Log("Input target language: ", newLine: false);
            string targetLanguage = Console.ReadLine();
            Logger.LogToFile(targetLanguage);

            Logger.Log("Input result file (relative to %cockatrice_data%/customsets): ", newLine: false);
            string resultFile = Console.ReadLine();
            Logger.LogToFile(resultFile);

            resultFile = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_CUSTOMSETS_FOLDERNAME, resultFile);


            translateCollection(collectionFile, sourceLanguage, targetLanguage);

            collectionFile.ExportToXmlFile(resultFile);

            Logger.Log("Translating done.");
            Logger.Log("");
        }

        private static void translateCollection(CollectionFile collectionFile, string sourceLanguage, string targetLanguage)
        {
            translationBuffer = new Dictionary<string, string>();

            int currentCount = 0;
            int targetCount = 0;

            if (collectionFile.Sets != null)
                targetCount += collectionFile.Sets.Length;

            if (collectionFile.Cards != null)
                targetCount += collectionFile.Cards.Length;

            DateTime start = DateTime.Now;

            // traduzir titulos sets
            if (collectionFile.Sets != null)
                foreach (CollectionSet set in collectionFile.Sets)
                {
                    set.Name += "." + targetLanguage;

                    set.LongName = translateName(set.LongName, sourceLanguage, targetLanguage);

                    currentCount++;
                    monitorTranslationTime(start, currentCount, targetCount);
                }

            // traduzir cartas (nomes, textos, tipos)
            if (collectionFile.Cards != null)
                foreach (CollectionCard card in collectionFile.Cards)
                {
                    card.Name = translateName(card.Name, sourceLanguage, targetLanguage);
                    card.Text = translateEntry(card.Text, sourceLanguage, targetLanguage);
                    card.Properties.MainType = translateType(card.Properties.MainType, sourceLanguage, targetLanguage);
                    card.Properties.Type = translateType(card.Properties.Type, sourceLanguage, targetLanguage);

                    foreach (CollectionCardPrint print in card.Prints)
                        print.SetName += ".pt";

                    currentCount++;
                    monitorTranslationTime(start, currentCount, targetCount);
                }
        }

        private static void monitorTranslationTime(DateTime start, int currentCount, int targetCount)
        {
            if (currentCount > 0 && currentCount % 50 == 0)
            {
                Logger.Log("");
                Logger.Log($"{currentCount} entries already translated. {targetCount - currentCount} left.");

                DateTime end = DateTime.Now;
                TimeSpan len = end - start;
                decimal ticksPerDownload = len.Ticks / currentCount;

                decimal remainingTimeTicks = (targetCount - currentCount) * ticksPerDownload;
                TimeSpan remainingTimeSpan = new TimeSpan((long)remainingTimeTicks);
                DateTime timeOfCompletion = DateTime.Now.Add(remainingTimeSpan);

                Logger.Log($"Estimated time for completion: {(int)remainingTimeSpan.TotalMinutes} minutes at {timeOfCompletion.ToString()}");
                Logger.Log("");
            }
        }

        private static string translateType(string entry, string sourceLanguage, string targetLanguage)
        {
            if (entry.StartsWith("Token "))
                entry = "Token - " + entry.Substring(6);

            if (translationBuffer.ContainsKey(entry))
                return translationBuffer[entry];

            string translation = translateEntry(entry, sourceLanguage, targetLanguage);

            translationBuffer.Add(entry, translation);

            return translation;
        }

        private static string translateName(string entry, string sourceLanguage, string targetLanguage)
        {
            int i = entry.IndexOf(": ");

            if (i==-1)
                i = entry.IndexOf(", ");

            if (i == -1)
            {
                return translateEntry(entry, sourceLanguage, targetLanguage);
            }
            else
            {
                string name = entry.Substring(0, i);
                entry = entry.Substring(i);

                return name + translateEntry(entry, sourceLanguage, targetLanguage, name);
            }
        }

        private static string translateEntry(string entry, string sourceLanguage, string targetLanguage, string optional = "")
        {
            if (entry == null)
                return null;

            int retries = 3;
            string translated = null;

            while (translated == null && retries > 0)
            {
                translated = Google.Translator.Translate(entry, sourceLanguage, targetLanguage);
                retries--;
                System.Threading.Thread.Sleep(500);
            }

            if (translated != null)
                Logger.Log($"Entry '{optional}{entry}' translated to '{optional}{translated}'.");
            else
                Logger.Log($"Entry '{optional}{entry}' was not translated!");

            return translated;
        }

        private static void createNewCockatriceCollection()
        {
            Logger.Log("Creating new Cokactrice collection...");

            Logger.Log("Input set code: ", newLine: false);
            string setCode = Console.ReadLine();
            Logger.LogToFile(setCode);

            Logger.Log("Input set name: ", newLine: false);
            string setName = Console.ReadLine();
            Logger.LogToFile(setName);

            Logger.Log("Input set type (\"Custom[: <game>[:<set>]]\"): ", newLine: false);
            string setType = Console.ReadLine();
            Logger.LogToFile(setType);

            Logger.Log("Input set author: ", newLine: false);
            string setAuthor = Console.ReadLine();
            Logger.LogToFile(setAuthor);

            Logger.Log("Input set version: ", newLine: false);
            string setVersion = Console.ReadLine();
            Logger.LogToFile(setVersion);

            Logger.Log("Input set date: ", newLine: false);
            string setDate = Console.ReadLine();
            Logger.LogToFile(setDate);

            Logger.Log("Input result file (relative to %cockatrice_data%/customsets): ", newLine: false);
            string resultFile = Console.ReadLine();
            Logger.LogToFile(resultFile);

            resultFile = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_CUSTOMSETS_FOLDERNAME, resultFile);

            CollectionFile newCockatriceCollection = new CollectionFile();

            newCockatriceCollection.Info = new CollectionInfo();
            newCockatriceCollection.Info.Author = setAuthor;
            newCockatriceCollection.Info.SourceVersion = setVersion;
            newCockatriceCollection.Info.CreatedAt = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");

            newCockatriceCollection.Sets = new CollectionSet[1];
            newCockatriceCollection.Sets[0] = new CollectionSet();
            newCockatriceCollection.Sets[0].Name = setCode;
            newCockatriceCollection.Sets[0].LongName = setName;
            newCockatriceCollection.Sets[0].SetType = setType;
            newCockatriceCollection.Sets[0].ReleaseDate = setDate;

            newCockatriceCollection.Cards = new CollectionCard[0];

            newCockatriceCollection.ExportToXmlFile(resultFile);

            Logger.Log("Creation done.");
            Logger.Log("");
        }

        private static void addDeckToAnotherCollection(CollectionFile collectionFile)
        {
            Logger.Log("Adding Cokactrice deck to another collection...");

            Logger.Log("Input deck filename (relative to %cockatrice_data%\\decks): ", newLine: false);
            string deckFilename = Console.ReadLine();

            Logger.Log("Input another collection filename (relative to %cockatrice_data%\\customsets): ", newLine: false);
            string anotherCollectionFilename = Console.ReadLine();



            deckFilename = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_DECKS_FOLDERNAME, deckFilename);
            anotherCollectionFilename = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_CUSTOMSETS_FOLDERNAME, anotherCollectionFilename);

            CollectionFile anotherCollection = CollectionFile.ImportFromXmlFile(anotherCollectionFilename);

            string[] allDeckLines = System.IO.File.ReadAllLines(deckFilename);

            int missing = 0;

            foreach(string deckLine in allDeckLines)
            {
                int i = deckLine.IndexOf(" ");
                string cardName = deckLine.Substring(i + 1);

                CollectionCard card = collectionFile.Cards.FirstOrDefault(x => x.Name == cardName);

                if (card == null)
                {
                    Logger.Log($"Card '{card.Name}' not found in collection!");
                    missing++;
                }
                else
                {
                    CollectionCard cardCopy = card.Clone();
                    CollectionCardPrint print = cardCopy.Prints[0];
                    cardCopy.Prints = new CollectionCardPrint[1];
                    cardCopy.Prints[0] = print;
                    cardCopy.Prints[0].SetName = anotherCollection.Sets[0].Name;
                    anotherCollection.Cards = anotherCollection.Cards.Append(cardCopy).ToArray();
                }
            }

            anotherCollection.ExportToXmlFile(anotherCollectionFilename);

            Logger.Log($"{allDeckLines.Length - missing} added to another collection.");
            Logger.Log($"{missing} cards not found!");

            Logger.Log("Adding done.");
            Logger.Log("");
        }

        private static void countCharacters(CollectionFile collectionFile)
        {
            Logger.Log("Counting titles and texts total length....");

            int titleLength = 0;
            int textLength = 0;
            int mainTypeLength = 0;
            int typeLength = 0;

            foreach (CollectionCard card in collectionFile.Cards)
            {
                titleLength += card.Name.Length;
                textLength += card.Text.Length;
                mainTypeLength += card.Properties.MainType.Length;
                typeLength += card.Properties.Type.Length;
            }

            Logger.Log($"Total titles length: {titleLength}");
            Logger.Log($"Total texts length: {textLength}");
            Logger.Log($"Total main types length: {mainTypeLength}");
            Logger.Log($"Total types length: {typeLength}");
            Logger.Log($"Total length: {titleLength + textLength + mainTypeLength + typeLength}");

            Logger.Log("Counting done.");
            Logger.Log("");

        }

        private static void batchConvertPlanesculptorsJsonToCockatriceXml()
        {
            Logger.Log("Batch converting sets from Planesculptors to Cockatrice XML...");

            Logger.Log("Input batch filename (relative to %cockatrice_data%\\customsets): ", newLine: false);
            string batchFilename = Console.ReadLine();
            Logger.LogToFile(batchFilename);

            Logger.Log("Type 'download' if you to want to download all images: ", newLine: false);
            bool toDownload = Console.ReadLine().ToLower() == "download";
            Logger.LogToFile(batchFilename);


            batchFilename = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_CUSTOMSETS_FOLDERNAME, batchFilename);

            string allText = System.IO.File.ReadAllText(batchFilename);
            string[] allLines = allText.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Logger.Log("");

            foreach (string line in allLines)
            {
                string[] allParameteres = line.Split('\t');

                convertPlanesculptorsJsonToCockatriceXml(allParameteres[0], allParameteres[1], allParameteres[2], allParameteres[3], allParameteres[4], allParameteres[5], allParameteres[6], allParameteres[7]);

                if (toDownload)
                {
                    Logger.Log("Downloading card images...");
                    Logger.Log("");

                    string collectionFilepath = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_CUSTOMSETS_FOLDERNAME, allParameteres[7]);
                    string downloadedPicsPath = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_COSTUM_IMAGE_FOLDERNAME);
                    CollectionFile collectionFile = CollectionFile.ImportFromXmlFile(collectionFilepath);
                    downloadCardImages(collectionFile, downloadedPicsPath);

                    Logger.Log("Card images downloaded.");
                    Logger.Log("");
                }
            }

            Logger.Log("Batch conversion done.");
            Logger.Log("");
        }

        private static void convertPlanesculptorsJsonToCockatriceXml()
        {
            Logger.Log("Converting set from Planesculptors to Cockatrice XML...");

            Logger.Log("Input set code: ", newLine: false);
            string setCode = Console.ReadLine();
            Logger.LogToFile(setCode);

            Logger.Log("Input set name: ", newLine: false);
            string setName = Console.ReadLine();
            Logger.LogToFile(setName);

            Logger.Log("Input set type (\"Custom[: <game>[:<set>]]\"): ", newLine: false);
            string setType = Console.ReadLine();
            Logger.LogToFile(setType);

            Logger.Log("Input set author: ", newLine: false);
            string setAuthor = Console.ReadLine();
            Logger.LogToFile(setAuthor);

            Logger.Log("Input set version: ", newLine: false);
            string setVersion = Console.ReadLine();
            Logger.LogToFile(setVersion);

            Logger.Log("Input set date: ", newLine: false);
            string setDate = Console.ReadLine();
            Logger.LogToFile(setDate);

            Logger.Log("Input PlaneSculptors page url for the set: ", newLine: false);
            string planesculptorsPageUrl = Console.ReadLine();
            Logger.LogToFile(planesculptorsPageUrl);

            Logger.Log("Input result file (relative to %cockatrice_data%/customsets): ", newLine: false);
            string resultFile = Console.ReadLine();
            Logger.LogToFile(resultFile);

            resultFile = Path.Combine(COCKATRICE_DATA_FOLDER, COCKATRICE_DEFAULT_CUSTOMSETS_FOLDERNAME, resultFile);

            convertPlanesculptorsJsonToCockatriceXml(setCode, setName, setType, DateTime.Parse(setDate).ToString("yyyy-MM-dd"), setAuthor, setVersion, planesculptorsPageUrl, resultFile);
        }

        private static void convertPlanesculptorsJsonToCockatriceXml(string setCode, string setName, string setType, string setAuthor, string setVersion, string setDate, string planesculptorsPageUrl, string resultFile)
        {
            Logger.Log($"Converting Planesculptors set '{setName}', '{setName}' into Cockatrice...");

            string json = SiteInterface.GetSetJson(planesculptorsPageUrl);
            CardList cardList = JsonConvert.DeserializeObject<CardList>(json);

            CollectionFile cockatriceCollection = PlanesculptorsToCockatriceConverter.Convert(cardList, setCode);

            cockatriceCollection.Sets[0].Name = setCode;
            cockatriceCollection.Sets[0].LongName = setName;
            cockatriceCollection.Sets[0].SetType = setType;
            cockatriceCollection.Sets[0].ReleaseDate = setDate;

            cockatriceCollection.Info.Author = setAuthor;
            cockatriceCollection.Info.SourceVersion = setVersion;
            cockatriceCollection.Info.SourceUrl = planesculptorsPageUrl;
            cockatriceCollection.Info.CreatedAt = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"); 

            cockatriceCollection.ExportToXmlFile(resultFile);

            Logger.Log("Conversion done.");
            Logger.Log("");
        }

        private static void printReportForCollectionFile(CollectionFile collectionFile)
        {
            Logger.Log("Collection loaded from xml file.");

            if (collectionFile.Info != null)
            {
                if (collectionFile.Info.Author != null)
                    Logger.Log($"Author: {collectionFile.Info.Author}.");

                if (collectionFile.Info.SourceVersion != null)
                    Logger.Log($"Version: {collectionFile.Info.SourceVersion}.");

            }
            else
            {
                Logger.Log("Set provide no info!");
            }

            if (collectionFile.Sets != null)
                Logger.Log($"{collectionFile.Sets.Count().ToString()} set(s).");

            if (collectionFile.Cards != null)
                Logger.Log($"{collectionFile.Cards.Count().ToString()} cards.");

            Logger.Log($"{collectionFile.PrintCount.ToString()} prints.");


            Logger.Log("");
        }

        private static void downloadCardImages(CollectionFile collectionFile, string rootFolder)
        {
            Logger.Log("Download card images from server...");

            createSetFolders(collectionFile, rootFolder);

            int missingImagesCount = countMissingImages(collectionFile, rootFolder);
            int downloadedPicsCount = 0;

            DateTime start = DateTime.Now;

            foreach (CollectionCard card in collectionFile.Cards)
            {
                foreach (CollectionCardPrint cardPrint in card.Prints)
                {
                    string cardName = fixCardName(card);
                    string cardPrintLocalFilename = Path.Combine(rootFolder, cardPrint.SetName, cardName + ".jpg");

                    if (!System.IO.File.Exists(cardPrintLocalFilename))
                    {
                        if (cardPrint.HasPrintImage)
                        {
                            Logger.Log($"Download image for card '{card.Name}' in set '{cardPrint.SetName}'.");

                            int retries = 5;
                            bool downloaded = false;

                            while (!downloaded && retries > 0)
                            {
                                downloaded = downloadCardPrintImage(card, cardPrint, cardPrintLocalFilename);
                                retries--;
                            }

                            if (downloaded)
                                downloadedPicsCount++;

                            // Kindly asked by scryfall between each request.
                            //System.Threading.Thread.Sleep(100);
                        }
                        else
                        {
                            Logger.Log($"Card '{card.Name}' in set '{cardPrint.SetName}' does not define an image!");
                        }

                        if (downloadedPicsCount > 0 && downloadedPicsCount % 50 == 0)
                        {
                            Logger.Log("");
                            Logger.Log($"{downloadedPicsCount} images already downloaded. {(missingImagesCount - downloadedPicsCount)} left.");

                            DateTime end = DateTime.Now;
                            TimeSpan len = end - start;
                            decimal ticksPerDownload = len.Ticks / downloadedPicsCount;
                            decimal remainingTimeTicks = (missingImagesCount - downloadedPicsCount) * ticksPerDownload;
                            TimeSpan remainingTimeSpan = new TimeSpan((long)remainingTimeTicks);
                            DateTime timeOfCompletion = DateTime.Now.Add(remainingTimeSpan);

                            Logger.Log($"Estimated time for completion: {(int)remainingTimeSpan.TotalMinutes} minutes at {timeOfCompletion.ToString()}");
                            Logger.Log("");
                        }

                    }
                }

            }

            Logger.Log("Download card images from server terminated.");
            Logger.Log("");

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
                    Logger.Log($"Web Exception! Http Code: {wex.Status} - {wex.Message}", wex);

                    switch ((int)wex.Status)
                    {
                        case 429:
                            // Too Many Requests
                            Logger.Log("Sleeping for 60 seconds...");
                            System.Threading.Thread.Sleep(60000);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Exception! {ex.Message}", ex);
                }
            }

            return false;
        }

        private static int countMissingImages(CollectionFile collectionFile, string rootFolder)
        {
            Logger.Log("Counting missing images...");

            int count = 0;

            foreach (CollectionCard card in collectionFile.Cards)
            {
                foreach (CollectionCardPrint cardPrint in card.Prints)
                {
                    string cardName = fixCardName(card);
                    string cardPrintLocalFilename = Path.Combine(rootFolder, cardPrint.SetName, cardName + ".jpg");

                    if (!System.IO.File.Exists(cardPrintLocalFilename))
                        count++;
                }
            }

            Logger.Log($"There are {count} missing images.");
            Logger.Log("");

            return count;
        }

        private static int nameMissingImages(CollectionFile collectionFile, string rootFolder)
        {
            Logger.Log("Naming missing images...");

            int count = 0;

            foreach (CollectionCard card in collectionFile.Cards)
            {
                foreach (CollectionCardPrint cardPrint in card.Prints)
                {
                    string cardFixedName = fixCardName(card);
                    string cardPrintLocalFilename = Path.Combine(rootFolder, cardPrint.SetName, cardFixedName + ".jpg");

                    if (!System.IO.File.Exists(cardPrintLocalFilename))
                    {
                        count++;
                        Logger.Log($"Image is missing for card '{card.Name}' in set '{cardPrint.SetName}'.");
                    }
                }
            }

            Logger.Log($"There are {count} missing images.");
            Logger.Log("");

            return count;
        }

        private static int countMissingImageDefintions(CollectionFile collectionFile, string rootFolder)
        {
            Logger.Log("Counting missing image definitions...");

            int count = 0;

            foreach (CollectionCard card in collectionFile.Cards)
                foreach (CollectionCardPrint cardPrint in card.Prints)
                    if (!cardPrint.HasPrintImage)
                        count++;

            Logger.Log($"There are {count} missing image definitions.");
            Logger.Log("");

            return count;
        }

        private static int nameMissingImageDefintions(CollectionFile collectionFile, string rootFolder)
        {
            Logger.Log("Naming missing images definitions...");

            int count = 0;

            foreach (CollectionCard card in collectionFile.Cards)
                foreach (CollectionCardPrint cardPrint in card.Prints)
                    if (!cardPrint.HasPrintImage)
                    {
                        count++;
                        Logger.Log($"Image definition is missing for card '{card.Name}' in set '{cardPrint.SetName}'.");
                    }

            Logger.Log($"There are {count} missing images definitions");
            Logger.Log("");

            return count;
        }

        private static void checkRootFolderExistence(string rootFolder)
        {
            if (!System.IO.Directory.Exists(rootFolder))
            {
                Logger.Log($"Create root folder '{rootFolder}'.");
                System.IO.Directory.CreateDirectory(rootFolder);
            }
        }

        private static void createSetFolders(CollectionFile collectionFile, string rootFolder)
        {
            Logger.Log("Creating set folders...");

            if (collectionFile.Sets == null)
            {
                Logger.Log("This collection does not contain sets.");
                return;
            }

            checkRootFolderExistence(rootFolder);

            foreach (CollectionSet set in collectionFile.Sets)
            {
                checkSetFolderExistence(rootFolder, set.Name);
            }

            Logger.Log("Set folders created.");
            Logger.Log("");

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
                Logger.Log($"Create folder for set '{folderName}'.");
                System.IO.Directory.CreateDirectory(folderName);
            }
        }
    }
}

