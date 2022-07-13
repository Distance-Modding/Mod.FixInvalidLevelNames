# Distance Fix Invalid Level Names

Distance mod to correct Workshop level names that are invalid, to prevent issues with Local Leaderboards replays and Steam Cloud sync.


## Current Options

* **Enable this Mod:** Activate options below for sanitizing level names, and redirect Local Leaderboards replay files into valid folders.
    * **Old folder name:** `./<LEVEL_NAME>/`
    * **New folder name:** `./.invalid/<WORKSHOP_LEVEL_ID>/`
	* Because this mod changes the location of replays for sanitized names, these replays will not be accessible without the mod.
* **Name Length Threshold:** Decide how long a level name can be, without needing to be sanitized. Any name longer than this count will be sanitized. Generally the default should be fine, but decreasing the max length can help if your Windows username is especially long.


### What this mod does:

* Fix Local Leaderboards folder names for **Workshop** levels only.
* Redirect all **future** Local Leaderboards replay files to a folder name that is valid.
* Force Local Leaderboards replay files to be for looked in the new folder (replays in the old folder, if any exist, will not show up).


### What this mod does not do:

* Move, delete, or rename any pre-existing files created with invalid level names.
* Fix invalid level **File** names (not replays), these would most likely fail to download in the first place.
* Fix Local Leaderboards folder names for **non-Workshop** levels.

**Note:** If you need to delete invalid folders, such as `"ma_ancient futuer "` with a trailing space, you can following the [instructions listed here](https://superuser.com/a/866217/925269).



***

## About invalid level names

### File locations

Distance save data is stored in:

* `<DOCUMENTS>/My Games/Distance/`

Workshop level files are stored in:

* `Levels/<LEVEL_TYPE>/<AUTHOR_ID>/<LEVEL_FILE_NAME>.bytes`
* `Levels/<LEVEL_TYPE>/<AUTHOR_ID>/<LEVEL_FILE_NAME>.bytes.png`

Personal level files are stored in:

* `Levels/<LEVEL_TYPE>/<LEVEL_FILE_NAME>.bytes`
* `Levels/<LEVEL_TYPE>/<LEVEL_FILE_NAME>.bytes.png`

Workshop Local Leaderboards replays are stored in:

* `LocalLeaderboards/<LEVEL_TYPE>/<AUTHOR_ID>/<LEVEL_FILE_NAME>/<MODE>_<GUID>.bytes`

And all other Local Leaderboards replays are stored in:

* `LocalLeaderboards/<LEVEL_TYPE>/<LEVEL_FILE_NAME>/<MODE>_<GUID>.bytes`


### What makes an invalid name

There are 3 main factors that can produce an invalid level name:

1. The name starts or ends with whitespace, or ends with a period. This causes non-fatal issues that just prevent replays from being saved. Windows will also complain when trying to copy or delete a file/folder name ending in a space.
2. The absolute file path is more than 260 characters long. This is an ancient limitation on Windows that is less relevant nowadays, but Windows file explorer will not be happy with it, and many older programs aren't written to handle lengths longer than 260. Based on the standard save data location for Distance Workshop replays, the **absolute maximum** file name length for a level can be 148 characters, and that's assuming your Windows username is only 1 character long.
3. The name is a reserved keyword, such as `COM1`, `NUL`, and `..`. This includes names starting with this keyword followed by a period, after that it doesn't matter what text follows the period, the name will be invalid. **Note:** This type of invalid name shouldn't be solvable with the mod, since the level file name itself wouldn't even be able to download.

### Issues caused by invalid names

Because Distance stores level files and Local Leaderboards replay files in paths that contain the level file name, any issues with this name can cause issues with:
* Downloading the level data
* Downloading the level image
* Saving Local Leaderboards replays
* Syncing Steam Cloud save data

<p align="center">

*Figure A: Completing MA\_Ancient Future, but no Local Leaderboards replay being created because of the trailing space in the folder name*

![MA_Ancient Future no local replay](https://i.imgur.com/plz8nTW.png)

</p>

<p align="center">

*Figure B: Trying to copy the Distance save data folder, but MA\_Ancient Future causes an error (which can be ignored)*

![MA_Ancient Future folder copy error](https://i.imgur.com/uQ3KuXj.png)

</p>

<p align="center">

*Figure C: Workshop level with a file name that is too long, causing Steam Cloud sync issues*

![Steam Cloud sync error](https://i.imgur.com/yMPQ0u5.png)

</p>



***

## Known levels with invalid names

* [MA_Ancient Future](https://steamcommunity.com/sharedfiles/filedetails/?id=2640449846)
    * **Reason:** Trailing - File name ends with a space (before extension).
	* **File name:** `ma_ancient future .bytes`
* [Framerate Crusher (Trackmogrify)](https://steamcommunity.com/sharedfiles/filedetails/?id=839128031)
    * **Reason:** Too Long - File name is 149 characters long (without extension).
    * **File name:** `trackmogrify_framerate crusher (insanely giant bright nightmare ultra tron city laser laser laser laser tron bright marathon transparent tron bright).bytes`
