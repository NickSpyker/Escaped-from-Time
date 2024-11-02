using System;
using System.Collections.Generic;
using Godot;

namespace EscapedfromTime.Objects.CharacterMeshes.Adventurers;

public partial class Barbarian : Node3D
{
    [Export] public AnimationPlayer AnimationPlayer;

    [ExportCategory("Barbarian items")]
    [Export] public BoneAttachment3D H1AxeOffhand;
    [Export] public BoneAttachment3D RoundShield;
    [Export] public BoneAttachment3D H1Axe;
    [Export] public BoneAttachment3D H2Axe;
    [Export] public BoneAttachment3D Mug;
    [Export] public BoneAttachment3D Hat;
    [Export] public BoneAttachment3D Cape;

    public enum Equipment
    {
        H1AxeOffhand,
        RoundShield,
        H1Axe,
        H2Axe,
        Mug,
        Hat,
        Cape
    }

    public HashSet<Equipment> Equipments { get; private set; } = new()
    {
        Equipment.H1AxeOffhand,
        Equipment.RoundShield,
        Equipment.H1Axe,
        Equipment.H2Axe,
        Equipment.Mug,
        Equipment.Hat,
        Equipment.Cape
    };

    public void ChangeEquipmentVisibility(Equipment equipment, bool enabled)
    {
        switch (equipment)
        {
            case Equipment.H1AxeOffhand:
                H1AxeOffhand.SetVisible(enabled);
                break;
            case Equipment.RoundShield:
                RoundShield.SetVisible(enabled);
                break;
            case Equipment.H1Axe:
                H1Axe.SetVisible(enabled);
                break;
            case Equipment.H2Axe:
                H2Axe.SetVisible(enabled);
                break;
            case Equipment.Mug:
                Mug.SetVisible(enabled);
                break;
            case Equipment.Hat:
                Hat.SetVisible(enabled);
                break;
            case Equipment.Cape:
                Cape.SetVisible(enabled);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(equipment), equipment, null);
        }

        if (enabled) Equipments.Add(equipment); else Equipments.Remove(equipment);
    }
}
