// Sonic '06 Character Swapper is licensed under the MIT License:
/* 
 * MIT License
 * 
 * Copyright (c) 2020 HyperBE32
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using Marathon.Helpers;
using Marathon.Components;
using Marathon.IO.Formats.Archives;
using S2006CharSwapMarathon.Helpers;

namespace S2006CharSwapMarathon
{
    public partial class Main : Form
    {
        // Path to the archive in application root.
        private string ArchivePath = Path.Combine(Program.WorkingDirectory, "player.arc");

        // List of valid character scripts.
        private List<ArchiveFile> Scripts = new List<ArchiveFile>();

        // Archive loaded into memory.
        private Archive LoadedArchive;

        // Random number generator for randomising the characters.
        private Random RandomNumberGenerator = new Random();

        // List of forbidden character scripts.
        private List<string> ForbiddenScripts = new List<string>()
        {
            "amigo", "animation", "collision", "common", "_damaged_and_wind", "particle", "_scale_camera"
        };

        public Main()
        {
            InitializeComponent();

            // Set version string.
            Label_Version.Text = $"Version {Program.Version}";

            // Restore last checked states.
            CheckBoxDark_CreateMod.Checked = Properties.Settings.Default.CreateMod;
            CheckBoxDark_OverwriteArchive.Checked = Properties.Settings.Default.OverwriteArchive;
        }

        /// <summary>
        /// Perform tasks upon form loading.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!File.Exists(ArchivePath))
            {
                MarathonMessageBox.Show
                (
                    "Please put 'player.arc' from your copy of SONIC THE HEDGEHOG into the root folder containing this application!",
                    "Missing Archive",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                Close();

                return;
            }

            // Get script data.
            GetLuaScripts();

            // Create random seed.
            TextBoxDark_Seed.Text = RandomNumberGenerator.Next().ToString();
        }

        /// <summary>
        /// Perform tasks upon form closing.
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Save updated settings.
            Properties.Settings.Default.Save();

            base.OnFormClosing(e);
        }

        /// <summary>
        /// Update UI and settings with checked state.
        /// </summary>
        private void CheckBoxDark_CreateMod_CheckedChanged(object sender, EventArgs e)
        {
            // Update settings.
            Properties.Settings.Default.CreateMod = CheckBoxDark_CreateMod.Checked;

            // Update UI.
            CheckBoxDark_OverwriteArchive.Enabled = !CheckBoxDark_CreateMod.Checked;
        }

        /// <summary>
        /// Update settings with checked state.
        /// </summary>
        private void CheckBoxDark_OverwriteArchive_CheckedChanged(object sender, EventArgs e)
        {
            // Update settings.
            Properties.Settings.Default.OverwriteArchive = CheckBoxDark_OverwriteArchive.Checked;
        }

        /// <summary>
        /// Loads the archive and gets the valid character scripts.
        /// </summary>
        private void GetLuaScripts()
        {
            // List of names for all scripts in the archive.
            List<string> scriptNames = new List<string>();
#if !DEBUG
            try
            {
#endif
                // Load 'player.arc' into memory.
                LoadedArchive = new U8Archive(ArchivePath);
#if !DEBUG
            }
            catch
            {
                MarathonMessageBox.Show
                (
                    "Failed to load 'player.arc' - the archive may be corrupt or invalid.",
                    "Archive Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                Close();

                return;
            }
#endif
            // Get all player Lua scripts.
            Scripts.AddRange
            (
                LoadedArchive.GetFiles().Where(x => !ForbiddenScripts.Any(x.Name.Contains) && Path.GetExtension(x.Name) == ".lub")
            );

            // Add all script names to the list.
            foreach (ArchiveFile file in Scripts)
                scriptNames.Add(file.Name);

            // Populate all combo boxes.
            foreach (GroupContainer group in Controls.OfType<GroupContainer>())
                PopulateComboBoxesInGroup(group, scriptNames);

            // Set default combo box strings.
            foreach (ArchiveFile script in Scripts)
            {
                switch (script.Name.ToLower())
                {
                    /* Due to some backwards decisions from our good friends at Microsoft,
                       DropDownLists cannot have pre-defined text and I'm not using standard DropDowns for this...
                    
                       You've been warned, this is one hefty switch statement, and I wont lie to you, there's another
                       one just below all of this mess. */

                    #region Characters
                    case "sonic_new.lub":
                        ComboBoxDark_Sonic.Text = script.Name;
                        break;

                    case "shadow.lub":
                        ComboBoxDark_Shadow.Text = script.Name;
                        break;

                    case "silver.lub":
                        ComboBoxDark_Silver.Text = script.Name;
                        break;

                    case "princess.lub":
                        ComboBoxDark_PrincessElise.Text = script.Name;
                        break;

                    case "sonic_fast.lub":
                        ComboBoxDark_SonicMachSpeed.Text = script.Name;
                        break;

                    case "supersonic.lub":
                        ComboBoxDark_SuperSonic.Text = script.Name;
                        break;

                    case "supershadow.lub":
                        ComboBoxDark_SuperShadow.Text = script.Name;
                        break;

                    case "supersilver.lub":
                        ComboBoxDark_SuperSilver.Text = script.Name;
                        break;

                    case "amy.lub":
                        ComboBoxDark_Amy.Text = script.Name;
                        break;

                    case "blaze.lub":
                        ComboBoxDark_Blaze.Text = script.Name;
                        break;

                    case "knuckles.lub":
                        ComboBoxDark_Knuckles.Text = script.Name;
                        break;

                    case "omega.lub":
                        ComboBoxDark_Omega.Text = script.Name;
                        break;

                    case "rouge.lub":
                        ComboBoxDark_Rouge.Text = script.Name;
                        break;

                    case "tails.lub":
                        ComboBoxDark_Tails.Text = script.Name;
                        break;
                    #endregion

                    #region Vehicles
                    case "snow_board.lub":
                        ComboBoxDark_SnowboardCrisisCity.Text = script.Name;
                        break;

                    case "snow_board_wap.lub":
                        ComboBoxDark_SnowboardWhiteAcropolis.Text = script.Name;
                        break;

                    case "shadow_bike.lub":
                        ComboBoxDark_Bike.Text = script.Name;
                        break;

                    case "shadow_glider.lub":
                        ComboBoxDark_Glider.Text = script.Name;
                        break;

                    case "shadow_hover.lub":
                        ComboBoxDark_Hovercraft.Text = script.Name;
                        break;

                    case "shadow_jeep.lub":
                        ComboBoxDark_Jeep.Text = script.Name;
                        break;

                    case "shadow_none.lub":
                        ComboBoxDark_None.Text = script.Name;
                        break;
                    #endregion

                    #region Bosses
                    case "sonic_boss_dr3.lub":
                        ComboBoxDark_EggWyvern.Text = script.Name;
                        break;

                    case "silver_boss_dr2.lub":
                        ComboBoxDark_EggGenesis.Text = script.Name;
                        break;

                    case "silver_boss_iblis1.lub":
                        ComboBoxDark_IblisPhase1.Text = script.Name;
                        break;

                    case "silver_boss_iblis3.lub":
                        ComboBoxDark_IblisPhase3.Text = script.Name;
                        break;

                    case "boss_sonic.lub":
                        ComboBoxDark_SonicBoss.Text = script.Name;
                        break;

                    case "boss_shadow.lub":
                        ComboBoxDark_ShadowBoss.Text = script.Name;
                        break;

                    case "boss_silver.lub":
                        ComboBoxDark_SilverBoss.Text = script.Name;
                        break;
                    #endregion

                    #region Artificial Intelligence
                    case "npc_sonic.lub":
                        ComboBoxDark_SonicNPC.Text = script.Name;
                        break;

                    case "npc_shadow.lub":
                        ComboBoxDark_ShadowNPC.Text = script.Name;
                        break;

                    case "npc_silver.lub":
                        ComboBoxDark_SilverNPC.Text = script.Name;
                        break;

                    case "npc_princess.lub":
                        ComboBoxDark_PrincessEliseNPC.Text = script.Name;
                        break;

                    case "npc_supersonic.lub":
                        ComboBoxDark_SuperSonicNPC.Text = script.Name;
                        break;

                    case "npc_supershadow.lub":
                        ComboBoxDark_SuperShadowNPC.Text = script.Name;
                        break;

                    case "npc_supersilver.lub":
                        ComboBoxDark_SuperSilverNPC.Text = script.Name;
                        break;

                    case "npc_amy.lub":
                        ComboBoxDark_AmyNPC.Text = script.Name;
                        break;

                    case "npc_blaze.lub":
                        ComboBoxDark_BlazeNPC.Text = script.Name;
                        break;

                    case "npc_knuckles.lub":
                        ComboBoxDark_KnucklesNPC.Text = script.Name;
                        break;

                    case "npc_omega.lub":
                        ComboBoxDark_OmegaNPC.Text = script.Name;
                        break;

                    case "npc_rouge.lub":
                        ComboBoxDark_RougeNPC.Text = script.Name;
                        break;

                    case "npc_tails.lub":
                        ComboBoxDark_TailsNPC.Text = script.Name;
                        break;
                    #endregion

                    #region Character Select
                    case "select_sonic.lub":
                        ComboBoxDark_SonicMenu.Text = script.Name;
                        break;

                    case "select_shadow.lub":
                        ComboBoxDark_ShadowMenu.Text = script.Name;
                        break;

                    case "select_silver.lub":
                        ComboBoxDark_SilverMenu.Text = script.Name;
                        break;

                    case "select_amy.lub":
                        ComboBoxDark_AmyMenu.Text = script.Name;
                        break;

                    case "select_blaze.lub":
                        ComboBoxDark_BlazeMenu.Text = script.Name;
                        break;

                    case "select_knuckles.lub":
                        ComboBoxDark_KnucklesMenu.Text = script.Name;
                        break;

                    case "select_omega.lub":
                        ComboBoxDark_OmegaMenu.Text = script.Name;
                        break;

                    case "select_rouge.lub":
                        ComboBoxDark_RougeMenu.Text = script.Name;
                        break;

                    case "select_tails.lub":
                        ComboBoxDark_TailsMenu.Text = script.Name;
                        break;
                    #endregion
                }
            }

            // Populates the combo boxes in the requested group with the script names.
            void PopulateComboBoxesInGroup(GroupContainer group, List<string> scripts)
            {
                // Update all combo box strings.
                foreach (ComboBoxDark comboBox in group.WorkingArea.Controls.OfType<ComboBoxDark>())
                    comboBox.Items.AddRange(scripts.ToArray());
            }
        }

        /// <summary>
        /// Resets all the Lua scripts upon clicking.
        /// </summary>
        private void ButtonDark_Reset_Click(object sender, EventArgs e)
            => GetLuaScripts();

        /// <summary>
        /// Randomises all of the combo box selected indices upon clicking.
        /// </summary>
        private void ButtonDark_Randomise_Click(object sender, EventArgs e)
        {
            // List of character scripts not to be included in randomisation.
            List<string> ForbiddenRandomisedScripts = new List<string>()
            {
                "bike", "boss", "glider", "hover", "jeep", "none", "npc", "select"
            };

            // Add standard forbidden scripts, just in case.
            ForbiddenRandomisedScripts.AddRange(ForbiddenScripts);

            // Sets the random generator to the text box seed.
            RandomNumberGenerator = new Random(TextBoxDark_Seed.Text.GetHashCode());

            // Randomise all combo boxes.
            foreach (GroupContainer group in Controls.OfType<GroupContainer>())
                RandomiseComboBoxesInGroup(group);

            // Randomises all indices in the requested group.
            void RandomiseComboBoxesInGroup(GroupContainer group)
            {
                if
                (
                    group == GroupContainer_CharacterSelect        ||
                    group == GroupContainer_ArtificialIntelligence ||
                    group == GroupContainer_Vehicles               ||
                    group == GroupContainer_Bosses
                )
                {
                    /* We don't want to randomise anything in
                       these groups as it may cause crashes. */
                    return;
                }

                // Randomise all indices.
                foreach (ComboBoxDark comboBox in group.WorkingArea.Controls.OfType<ComboBoxDark>())
                {
                    // Generate new index.
                    int newIndex = RandomNumberGenerator.Next(0, comboBox.Items.Count);

                    // Check to ensure the index is valid and not forbidden.
                    if (!ForbiddenRandomisedScripts.Any(comboBox.Items[newIndex].ToString().Contains))
                    {
                        // Set the index to a random and valid one.
                        comboBox.SelectedIndex = newIndex;
                    }
                }
            }
        }

        /// <summary>
        /// Randomises the seed in the text box upon clicking.
        /// </summary>
        private void ButtonDark_RandomiseSeed_Click(object sender, EventArgs e)
            => TextBoxDark_Seed.Text = RandomNumberGenerator.Next().ToString();

        /// <summary>
        /// Process character swaps upon clicking.
        /// </summary>
        private void ButtonDark_Swap_Click(object sender, EventArgs e)
        {
            // Used for storage.
            ArchiveDirectory player = null;

            if (!File.Exists(ArchivePath))
            {
                MarathonMessageBox.Show
                (
                    "The original archive loaded has been removed - unable to decompress contents.",
                    "Missing Archive",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                Close();

                return;
            }
#if !DEBUG
            try
            {
#endif
                // Decompress all data.
                LoadedArchive.Decompress(ref LoadedArchive.Data);
#if !DEBUG
            }
            catch
            {
                MarathonMessageBox.Show
                (
                    "Failed to decompress 'player.arc' - the archive may be corrupt or invalid.",
                    "Archive Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                Close();

                return;
            }
#endif
            // Check if 'xenon\player\' returns null.
            if ((player = LoadedArchive.JumpToDirectory(@"xenon\player\")) == null)
            {
                // If so, then check if 'ps3\player\' returns null.
                if ((player = LoadedArchive.JumpToDirectory(@"ps3\player\")) == null)
                {
                    MarathonMessageBox.Show
                    (
                        "Failed to locate the scripts directory...",
                        "Archive Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );

                    Close();
                }
            }

            for (int i = 0; i < player.TotalContentsCount; i++)
            {
                ArchiveFile script = (ArchiveFile)player.Data[i];

                switch (script.Name.ToLower())
                {
                    /* Due to some backwards decisions from our good friends at Microsoft,
                       DropDownLists cannot have pre-defined text and I'm not using standard DropDowns for this...

                       I did warn you above. */

                    #region Characters
                    case "sonic_new.lub":
                        ReplaceScriptData(script, ComboBoxDark_Sonic);
                        break;

                    case "shadow.lub":
                        ReplaceScriptData(script, ComboBoxDark_Shadow);
                        break;

                    case "silver.lub":
                        ReplaceScriptData(script, ComboBoxDark_Silver);
                        break;

                    case "princess.lub":
                        ReplaceScriptData(script, ComboBoxDark_PrincessElise);
                        break;

                    case "sonic_fast.lub":
                        ReplaceScriptData(script, ComboBoxDark_SonicMachSpeed);
                        break;

                    case "supersonic.lub":
                        ReplaceScriptData(script, ComboBoxDark_SuperSonic);
                        break;

                    case "supershadow.lub":
                        ReplaceScriptData(script, ComboBoxDark_SuperShadow);
                        break;

                    case "supersilver.lub":
                        ReplaceScriptData(script, ComboBoxDark_SuperSilver);
                        break;

                    case "amy.lub":
                        ReplaceScriptData(script, ComboBoxDark_Amy);
                        break;

                    case "blaze.lub":
                        ReplaceScriptData(script, ComboBoxDark_Blaze);
                        break;

                    case "knuckles.lub":
                        ReplaceScriptData(script, ComboBoxDark_Knuckles);
                        break;

                    case "omega.lub":
                        ReplaceScriptData(script, ComboBoxDark_Omega);
                        break;

                    case "rouge.lub":
                        ReplaceScriptData(script, ComboBoxDark_Rouge);
                        break;

                    case "tails.lub":
                        ReplaceScriptData(script, ComboBoxDark_Tails);
                        break;
                    #endregion

                    #region Vehicles
                    case "snow_board.lub":
                        ReplaceScriptData(script, ComboBoxDark_SnowboardCrisisCity);
                        break;

                    case "snow_board_wap.lub":
                        ReplaceScriptData(script, ComboBoxDark_SnowboardWhiteAcropolis);
                        break;

                    case "shadow_bike.lub":
                        ReplaceScriptData(script, ComboBoxDark_Bike);
                        break;

                    case "shadow_glider.lub":
                        ReplaceScriptData(script, ComboBoxDark_Glider);
                        break;

                    case "shadow_hover.lub":
                        ReplaceScriptData(script, ComboBoxDark_Hovercraft);
                        break;

                    case "shadow_jeep.lub":
                        ReplaceScriptData(script, ComboBoxDark_Jeep);
                        break;

                    case "shadow_none.lub":
                        ReplaceScriptData(script, ComboBoxDark_None);
                        break;
                    #endregion

                    #region Bosses
                    case "sonic_boss_dr3.lub":
                        ReplaceScriptData(script, ComboBoxDark_EggWyvern);
                        break;

                    case "silver_boss_dr2.lub":
                        ReplaceScriptData(script, ComboBoxDark_EggGenesis);
                        break;

                    case "silver_boss_iblis1.lub":
                        ReplaceScriptData(script, ComboBoxDark_IblisPhase1);
                        break;

                    case "silver_boss_iblis3.lub":
                        ReplaceScriptData(script, ComboBoxDark_IblisPhase3);
                        break;

                    case "boss_sonic.lub":
                        ReplaceScriptData(script, ComboBoxDark_SonicBoss);
                        break;

                    case "boss_shadow.lub":
                        ReplaceScriptData(script, ComboBoxDark_ShadowBoss);
                        break;

                    case "boss_silver.lub":
                        ReplaceScriptData(script, ComboBoxDark_SilverBoss);
                        break;
                    #endregion

                    #region Artificial Intelligence
                    case "npc_sonic.lub":
                        ReplaceScriptData(script, ComboBoxDark_SonicNPC);
                        break;

                    case "npc_shadow.lub":
                        ReplaceScriptData(script, ComboBoxDark_ShadowNPC);
                        break;

                    case "npc_silver.lub":
                        ReplaceScriptData(script, ComboBoxDark_SilverNPC);
                        break;

                    case "npc_princess.lub":
                        ReplaceScriptData(script, ComboBoxDark_PrincessEliseNPC);
                        break;

                    case "npc_supersonic.lub":
                        ReplaceScriptData(script, ComboBoxDark_SuperSonicNPC);
                        break;

                    case "npc_supershadow.lub":
                        ReplaceScriptData(script, ComboBoxDark_SuperShadowNPC);
                        break;

                    case "npc_supersilver.lub":
                        ReplaceScriptData(script, ComboBoxDark_SuperSilverNPC);
                        break;

                    case "npc_amy.lub":
                        ReplaceScriptData(script, ComboBoxDark_AmyNPC);
                        break;

                    case "npc_blaze.lub":
                        ReplaceScriptData(script, ComboBoxDark_BlazeNPC);
                        break;

                    case "npc_knuckles.lub":
                        ReplaceScriptData(script, ComboBoxDark_KnucklesNPC);
                        break;

                    case "npc_omega.lub":
                        ReplaceScriptData(script, ComboBoxDark_OmegaNPC);
                        break;

                    case "npc_rouge.lub":
                        ReplaceScriptData(script, ComboBoxDark_RougeNPC);
                        break;

                    case "npc_tails.lub":
                        ReplaceScriptData(script, ComboBoxDark_TailsNPC);
                        break;
                    #endregion

                    #region Character Select
                    case "select_sonic.lub":
                        ReplaceScriptData(script, ComboBoxDark_SonicMenu);
                        break;

                    case "select_shadow.lub":
                        ReplaceScriptData(script, ComboBoxDark_ShadowMenu);
                        break;                                                
                                                                              
                    case "select_silver.lub":                                 
                        ReplaceScriptData(script, ComboBoxDark_SilverMenu);
                        break;                                                
                                                                              
                    case "select_amy.lub":                                    
                        ReplaceScriptData(script, ComboBoxDark_AmyMenu);
                        break;                                                
                                                                              
                    case "select_blaze.lub":                                  
                        ReplaceScriptData(script, ComboBoxDark_BlazeMenu);
                        break;

                    case "select_knuckles.lub":
                        ReplaceScriptData(script, ComboBoxDark_KnucklesMenu);
                        break;

                    case "select_omega.lub":
                        ReplaceScriptData(script, ComboBoxDark_OmegaMenu);
                        break;

                    case "select_rouge.lub":
                        ReplaceScriptData(script, ComboBoxDark_RougeMenu);
                        break;

                    case "select_tails.lub":
                        ReplaceScriptData(script, ComboBoxDark_TailsMenu);
                        break;
                        #endregion
                }
            }

            // Replace script data with the selected one.
            void ReplaceScriptData(ArchiveFile script, ComboBox comboBox)
            {
                // Find script entered into the combo box.
                ArchiveFile scriptToUse = Scripts.Find(x => x.Name == comboBox.Text);

                if (script.Name != scriptToUse.Name)
                {
                    // Create replacement file with the original name and replacement data.
                    var replacement = new U8Archive.U8ArchiveFile(scriptToUse)
                    {
                        Name = script.Name
                    };

                    // Replace the script with the requested replacement.
                    LoadedArchive.ReplaceFile(script, replacement);
                }
            }

            // Write swapped character archive.
            WriteFinalisedArchive();
        }

        /// <summary>
        /// Writes the final changes made by the user.
        /// </summary>
        private void WriteFinalisedArchive()
        {
#if !DEBUG
            try
            {
#endif
                if (CheckBoxDark_CreateMod.Checked)
                {
                           // Seed used for randomisation - use anyway, even if not randomised.
                    string seed = TextBoxDark_Seed.Text.UseSafeFormattedCharacters(),

                           // Root of the mod directory.
                           modRoot = Path.Combine(Program.WorkingDirectory, $"Character Swap ({seed})"),

                           // Full path to archives for directory creation.
                           modFullPath = Path.Combine(modRoot, "core", "archives");

                    // Create mod structure.
                    Directory.CreateDirectory(modFullPath);

                    // Write mod config with seed.
                    ModHelper.WriteConfig(Path.Combine(modRoot, "mod.ini"), seed);

                    // Save archive as mod with swapped characters.
                    LoadedArchive.Save(Path.Combine(modFullPath, "player.arc"));
                }
                else
                {
                    if (CheckBoxDark_OverwriteArchive.Checked)
                    {
                        // Overwrite original archive with swapped characters.
                        LoadedArchive.Save(ArchivePath, true);
                    }
                    else
                    {
                        // Write to new archive with swapped characters.
                        LoadedArchive.Save($"{ArchivePath}.swap", true);
                    }
                }

                MarathonMessageBox.Show
                (
                    "Finished processing your character swap - have fun!",
                    "Processing Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
#if !DEBUG
            }
            catch (Exception ex)
            {
                MarathonMessageBox.Show
                (
                    $"Failed to save archive data...\n\n{ex}",
                    "Fatal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
#endif
        }
    }
}
