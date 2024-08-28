# CCIL
Cockatrice Card Image Loader

Cockatrice Card Image Loader is a little console app coded in C# that allows:
* Download card images from Scryfall to a local folder so you don't need an internet connection to see card faces.
* Convert PlaneSculptors custom sets to Cockatrice xml files so you can play on your computer all those wonderful custom sets.

> Disclaimer:
> This tool is a simple console app developed in a short period of time without extensive testing and validation.
> So, if you don't follow the right steps, you might find some surprises.
> Even so, no Cockatrice file will ever be 'harmed' in the process.
> To the present moment, it was used to convert a dozen of the Magnitar Star Wars custom sets without any problem.
> But use this tool at your own risk ;)

> For the commit rules of this project please refer to the [commit rules file](Commits.md "Commit rules file").

## Setup

You just need to copy distribution files into a local folder and run "CockatriceCardImageLoader.exe".
Don't forget to customize the configuration file.

### CockatriceCardImageLoader.dll.config


| Parameter 					| Purpose 												|
|-----------------------------------------------|-------------------------------------------------------------------------------------------------------|
| DEFAULT_CARD_IMAGE_SERVER 			| Defines a URL for the card image server to use in case a card does not define a "picURL" attribute. 	|
| COCKATRICE_DATA_FOLDER 			| Defines the Cockatrice user folder. 									|
| COCKATRICE_DEFAULT_CARD_FILENAME 		| Defines the Cockatrice name for the default MTG card collection XML file.				|
| COCKATRICE_DEFAULT_TOKEN_FILENAME 		| Defines the Cockatrice name for the default MTG token collection XML file. 				|
| COCKATRICE_DEFAULT_CUSTOMSETS_FOLDERNAME 	| Defines the Cockatrice folder name where custom sets are found. 					|
| COCKATRICE_DEFAULT_IMAGE_FOLDERNAME 		| Defines the Cockatrice folder name for MTG card images. 						|
| COCKATRICE_COSTUM_IMAGE_FOLDERNAME 		| Defines the Cockatrice folder name for non-MTG card images. 						|

## Downloading card images.

1. In order to download card images, you first need to select the collection of cards to use.
2. You can select the default files for MTG cards, or you can input a custom file for a custom set.
3. Once a collection is selected, you have some operations available for that collection.
	1. Counting and naming missing card images.
	2. Counting and naming cards that do not define an image.
4. Download card images.
5. If a card defines a complete url for its image, the image is downloaded from the given url.
6. If a card does not define a complete URL, the tool will attempt to find it on the card image server defined in the config file. 

> The default card image server defined in the config file (Scryfall) is tailored for MTG, so it won't work with custom sets.
> In the case of custom sets, cards must define a complete URL for the card image.

## Converting PlaneSculptors custom sets to Cockatrice

1. Converting a PlaneSculptors custom set into the Cockatrice XML format is as easy as inputing some text parameters.
	1. The custom set page url from the PlaneSculptors web site.
	2. Set the code, name, release date, and author.
2. An XML file will be generated in the Cockatrice custom sets folder.
3. Next time you open Cockatrice, the custom set will be recognized and will be ready to use.
4. If you want to download the card images to a local folder, you just need to proceed the same way you would with other MTG sets.

### Mana cost compatibility

When converting from PlaneSculptors to Cockatrice, special attention must be given to the mana costs of cards and abilities.
Because the few tests this tool endured were done with the Magnitar Star Wars custom sets, its use of mana cost symbols determined the initial support of this tool.
For an extensive view of the costs supported, please open the file 'PlanesculptorsToCockatriceConverter.cs' in the source folder "Convertors".