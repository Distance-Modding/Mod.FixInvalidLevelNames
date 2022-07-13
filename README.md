# Distance Fix Invalid Level Names

Distance mod to correct Workshop level names that are invalid, to prevent issues with Local Leaderboards replays and Steam Cloud sync.


## Current Options

* **Enable this Mod:** Activate options below for sanitizing level names, and redirect Local Leaderboards replay files into valid folders.
    * **Old folder name:** `./<LEVEL_NAME>/`
    * **New folder name:** `./.invalid/<WORKSHOP_LEVEL_ID>/`
* **Sanitize Reserved Names:** Sanitize reserved names on Windows such as `COM1`, `NUL`, `..`, etc.
* **Sanitize Trailing Names:** Sanitize names with invalid trailing characters (starting or ending with whitespace, or ending with a period).
* **Sanitize Long Names:** Sanitize names that are too long, as described by the *Max Level Name Length* option.
* **Max Level Name Length:** Decide how long a level name can be before it will be sanitized. Generally the default should be fine, but decreasing the max length can help if your Windows username is especially long.


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

### Issues

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
