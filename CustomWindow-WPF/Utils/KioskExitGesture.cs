﻿namespace CustomWindow_WPF.Utils;

using System;
using System.Text;
using System.Windows.Input;

/// <summary>
/// This class represents a keyboard key and its modifiers.
/// </summary>
public class KioskExitGesture
{
    /// <summary>
    /// Gets the ModifierKeys.
    /// </summary>
    public ModifierKeys[] ModifierKeys { get; init; } = Array.Empty<ModifierKeys>();

    /// <summary>
    /// Gets the Key.
    /// </summary>
    public Key Key { get; init; } = Key.None;

    /// <summary>
    /// Initializes a new instance of the <see cref="KioskExitGesture"/> class.
    /// </summary>
    public KioskExitGesture() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="KioskExitGesture"/> class.
    /// </summary>
    /// <param name="key">The key<see cref="Key"/>.</param>
    /// <param name="modifierKeys">The modifierKeys<see cref="ModifierKeys"/>.</param>
    public KioskExitGesture(Key key, ModifierKeys[] modifierKeys)
    {
        Key = key;
        ModifierKeys = modifierKeys;
    }

    /// <summary>
    /// The ToString.
    /// </summary>
    /// <returns>The <see cref="string"/>.</returns>
    public override string ToString()
    {
        StringBuilder sb = new();

        foreach (ModifierKeys modifier in ModifierKeys)
            _ = sb.Append(modifier.ToString()).Append(" + ");

        _ = sb.Append(Key.ToString());

        return sb.ToString();
    }
}