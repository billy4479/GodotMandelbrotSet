using System;
using Godot;

public class Slider : HSlider {
    public override void _Ready() {
        Connect("value_changed", this, "OnChange");
    }

    public void OnChange(float value) {
        GetNode<Label>("/root/Node2D/Label").Text = "Max Iterations: " + value;
        (GetNode<Sprite>("/root/Node2D/Sprite").Material as ShaderMaterial).SetShaderParam("MAX_ITERATIONS", value);
    }
}