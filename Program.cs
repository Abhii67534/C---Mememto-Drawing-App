using System;
using System.Collections.Generic;
using System.Linq;

// Shape Interface
public interface IShape
{
    void Draw();
}

// Circle Class implementing Shape interface
public class Circle : IShape
{
    private string color;
    private int radius;

    public Circle(string color, int radius)
    {
        this.color = color;
        this.radius = radius;
    }

    public void Draw()
    {
        Console.WriteLine($"Drawing a {color} circle with radius {radius}");
    }
}

// Square Class implementing Shape interface
public class Square : IShape
{
    private string color;
    private int sideLength;

    public Square(string color, int sideLength)
    {
        this.color = color;
        this.sideLength = sideLength;
    }

    public void Draw()
    {
        Console.WriteLine($"Drawing a {color} square with side length {sideLength}");
    }
}

// Memento Class to store the state of the canvas
public class CanvasMemento
{
    private List<IShape> shapes;

    public CanvasMemento(List<IShape> shapes)
    {
        this.shapes = new List<IShape>(shapes);
    }

    public List<IShape> GetShapes()
    {
        return shapes;
    }
}

// Originator Class - Canvas
public class Canvas
{
    private List<IShape> shapes = new List<IShape>();

    public void AddShape(IShape shape)
    {
        shapes.Add(shape);
    }

    public void RemoveShape(IShape shape)
    {
        shapes.Remove(shape);
    }

    public void Draw()
    {
        Console.WriteLine("Canvas state:");
        foreach (var shape in shapes)
        {
            shape.Draw();
        }
        Console.WriteLine();
    }

    // Save current state to memento
    public CanvasMemento Save()
    {
        return new CanvasMemento(shapes);
    }

    // Restore state from memento
    public void Restore(CanvasMemento memento)
    {
        shapes = memento.GetShapes();
    }

    // Get the last shape added
    public IShape? GetLastShape() // Note the nullable return type
    {
        return shapes.LastOrDefault(); // This can return null if there are no shapes
    }
}

// Caretaker Class to manage undo functionality
public class CanvasHistory
{
    private Stack<CanvasMemento> history = new Stack<CanvasMemento>();

    public void Save(Canvas canvas)
    {
        history.Push(canvas.Save());
    }

    public void Undo(Canvas canvas)
    {
        if (history.Count > 0)
        {
            var lastSavedState = history.Pop();
            canvas.Restore(lastSavedState);
        }
        else
        {
            Console.WriteLine("No states to undo.");
        }
    }
}

// Demo
class Program
{
    static void Main()
    {
        var canvas = new Canvas();
        var history = new CanvasHistory();

        // Add first shape (a red circle)
        var circle = new Circle("red", 10);
        canvas.AddShape(circle);
        history.Save(canvas);  // Save state
        canvas.Draw();

        // Add second shape (a blue square)
        var square = new Square("blue", 20);
        canvas.AddShape(square);
        history.Save(canvas);  // Save state
        canvas.Draw();

        // Undo the last action (remove the square)
        Console.WriteLine("Undoing last action...");
        var lastShape = canvas.GetLastShape(); // Get the last added shape
        if (lastShape != null) // Check for null before removing
        {
            canvas.RemoveShape(lastShape); // Remove it
            history.Save(canvas); // Save the state after removal
            canvas.Draw(); // Check the state after undo
        }
        else
        {
            Console.WriteLine("No shapes to undo.");
        }

        // Undo the last action again (remove the circle)
        Console.WriteLine("Undoing last action...");
        lastShape = canvas.GetLastShape(); // Get the last shape again
        if (lastShape != null) // Ensure that we have a shape
        {
            canvas.RemoveShape(lastShape); // Remove it
            history.Save(canvas); // Save the state after removal
            canvas.Draw(); // Check the state after undo
        }
        else
        {
            Console.WriteLine("No shapes to undo.");
        }
    }
}
