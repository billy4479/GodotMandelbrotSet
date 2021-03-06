using System;
using Godot;

public class Movement : Sprite {

    Vector2 minCoords = new Vector2(-2.5f, -1f);
    Vector2 maxCoords = new Vector2(1f, 1f);
    ShaderMaterial shader;

    const float zoomSpeed = 0.05f;
    const float moveSpeed = 0.05f;

    private float AspectRatio {
        get { return GetViewportRect().Size.x / GetViewportRect().Size.y; }
    }
    private Vector2 Factor {
        get { return new Vector2((maxCoords.x - minCoords.x) / AspectRatio, maxCoords.y - minCoords.y); }
    }

    public override void _Ready() {
        var root = GetNode("/root");
        root.Connect("size_changed", this, "Resize");
        shader = Material as ShaderMaterial;
        SetProcessInput(true);
    }

    public void Resize() {
        var root = GetNode("/root");
        var resolution = root.GetViewport().GetVisibleRect();
        GD.Print(resolution);
        shader.SetShaderParam("screenSize", resolution.Size);
    }

    private void SetShaderCoords() {
        shader.SetShaderParam("minCoords", minCoords);
        shader.SetShaderParam("maxCoords", maxCoords);
    }

    public override void _Input(InputEvent @event) {
        if (@event is InputEventKey eventKey) {
            if (eventKey.Pressed && eventKey.Scancode == (int) KeyList.Escape) {
                GetTree().SetInputAsHandled();
                GetTree().Quit();
            }
            if (eventKey.Scancode == (int) KeyList.Up) {
                GetTree().SetInputAsHandled();
                minCoords.y += moveSpeed * Factor.y;
                maxCoords.y += moveSpeed * Factor.y;
                SetShaderCoords();
            }
            if (eventKey.Scancode == (int) KeyList.Down) {
                GetTree().SetInputAsHandled();
                minCoords.y -= moveSpeed * Factor.y;
                maxCoords.y -= moveSpeed * Factor.y;
                SetShaderCoords();
            }
            if (eventKey.Scancode == (int) KeyList.Right) {
                GetTree().SetInputAsHandled();
                minCoords.x += moveSpeed * Factor.x;
                maxCoords.x += moveSpeed * Factor.x;
                SetShaderCoords();
            }
            if (eventKey.Scancode == (int) KeyList.Left) {
                GetTree().SetInputAsHandled();
                minCoords.x -= moveSpeed * Factor.x;
                maxCoords.x -= moveSpeed * Factor.x;
                SetShaderCoords();
            }
        }
        if (@event is InputEventMouseButton eventMouse) {
            if (eventMouse.ButtonIndex == (int) ButtonList.WheelUp) {
                GetTree().SetInputAsHandled();
                minCoords += new Vector2(AspectRatio * zoomSpeed, zoomSpeed) * Factor;
                maxCoords -= new Vector2(AspectRatio * zoomSpeed, zoomSpeed) * Factor;
                SetShaderCoords();
            } else if (eventMouse.ButtonIndex == (int) ButtonList.WheelDown) {
                GetTree().SetInputAsHandled();
                minCoords -= new Vector2(AspectRatio * zoomSpeed, zoomSpeed) * Factor;
                maxCoords += new Vector2(AspectRatio * zoomSpeed, zoomSpeed) * Factor;
                SetShaderCoords();
            }
        }
    }
}