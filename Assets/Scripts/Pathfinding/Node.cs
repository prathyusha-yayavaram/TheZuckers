public class Node {

    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public int value;

    public Node cameFromNode;
    
    public Node(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public override string ToString() {
        return x + "," + y;
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }
}