# MainMenuReplacer

A tiny BepInEx plugin for Nuclear Option that swaps the main menu background with your own PNG.

---

## Installation

1. **Install BepInEx**  
   - Download BepInEx 5.4.x for Unity 2022.3 (Windows).  
   - Extract it into your `…\Nuclear Option\` folder so that `BepInEx\core\BepInEx.dll` and `BepInEx\core\0Harmony.dll` exist.

2. **Copy the plugin**  
   - Download `MainMenuReplacer.dll`.  
   - Place it in:  
     ```
     <Nuclear Option folder>\BepInEx\plugins\
     ```

3. **Add your PNG**  
   - Create a PNG named exactly `custom_menu.png`.  
   - Copy it into the same folder as the DLL:  
     ```
     <Nuclear Option folder>\BepInEx\plugins\custom_menu.png
     ```

4. **Launch the game**  
   - The plugin will detect and replace the full-screen “Background” Image in the Main Menu with `custom_menu.png`.  
   - Check the BepInEx console for confirmation messages.

---

That’s it—enjoy your custom main menu!
