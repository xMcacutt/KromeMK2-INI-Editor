# Krome MK2 INI Editor
This app was designed to make modifying the lv3 format easier but quickly evolved into a tool to view, modify, and export any file using Krome's ini format
It is a complex format so this converter does not produce an identical file from an unchanged input.
However, the files that are produced are valid and can be loaded by the Krome MK2 engine used for Ty the Tasmanian Tiger 2.

For Ty the Tasmanian Tiger 2, the following formats are considered valid ini formats:

.lv3, .ini, .model, .sound, .mad, .bni, .ui

## Features

### File Loading
Any .txt or valid ini file can be loaded into the editor. If a .txt file is opened, the text will be sent directly to the editor window. If a .lv3 or .bni file is opened, 
the file will be converted to a text format and then sent to the editor window.

### File Saving
The file must be a valid ini format for the save to succeed without crashing or creating a corrupted file. 

There are 3 options when saving a file.

#### Save Text
This will save the text in the editor window to a .txt file to make editing easy and fast in the future.

#### Export As INI
This will compile the text in the editor window into an ini file.

Note that for the game to load these files they must have the extension .bni after their normal extension.

Some files are temperamental on the most recent version of the game. UI files in particular are very sensitive to changes and will often crash the game.

This save option should primarily be used for collating completed edits for multiple files ready for repacking them into a .rkv.

#### Export As Test RKV
This will compile the text in the editor window into an ini file and then repack the file into a .rkv file for testing.

Note that this is a simplified version of the RKV2_Tools repacking tool and does not support multi-file repacking at all.
You will have to export each file individually and then use the RKV2_Tools repacking tool to repack the files into a .rkv file.

### Editing
The editor window is a simple text editor with some extra features to make editing the lv3 format easier.

Firstly, the editor uses syntax highlighting to show the different types of data in the lv3 format.

The lv3s are divided up into sections. Each section represents a game object or data block within the game world.
Each section has a name, some amount of fields, and some amount of sub-sections.
The fields have names and values and the sub-sections can have nested subsections with fields and values.

The syntax highlighting differentiates between
* Normal text
* Section names
* Field Names
* Field Text
* Numbers
* Keywords
Whilst not comprehensive, the syntax highlighting generally does a good job of aiding in
the readability of the format.

The editor also has an autocomplete feature which allows you to see all section and field names
listed across all of the vanilla ini files.

![sc2](Ty2INIEditor/ScreenShots/sc2.png)

### Customization
The editor's color scheme can be completely customized from the preferences menu.

There are 4 default themes but additional themes can be created by manually modifying the colors
and then saving a theme file. 

![sc3](Ty2INIEditor/ScreenShots/sc3.png)

DO NOT OVERWRITE COLORS.json

Here are the default themes:

Dark:

![sc1](Ty2INIEditor/ScreenShots/sc1.png)

Light:

![sc4](Ty2INIEditor/ScreenShots/sc4.png)

Mint:

![sc5](Ty2INIEditor/ScreenShots/sc5.png)

SuperDark:

![sc6](Ty2INIEditor/ScreenShots/sc6.png)

## Acknowledgements

This project would not have been possible without the help of the following people:

* Chippy - https://github.com/1superchip for writing the original parsing tool.
* Pavlo Torhashov - https://github.com/PavelTorgashov for FCTB used in the editor.
* Reza Aghaei - https://www.reza-aghaei.com for [this](https://stackoverflow.com/questions/65976232/how-to-change-the-combobox-dropdown-button-color/65976649#65976649) answer on SO