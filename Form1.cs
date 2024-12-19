namespace GraphAnalisV2
{
    // ���������� ����� ����� ����������
    public partial class Form1 : Form
    {
        // ����������� �����, ���������� ������������� �����������
        public Form1()
        {
            InitializeComponent(); // �������������� ���������� ����� (������ �������� ����������)
        }

        private int[,] adjacencyMatrix; // ��������� ���������� ��� �������� ������� ��������� �����
        private int n; // ����� ������ �����
        private List<int> setA = new List<int>(); // ������ ��� �������� ������ ������ ���� (��� ����������� �����)
        private List<int> setB = new List<int>(); // ������ ��� �������� ������ ������ ���� (��� ����������� �����)

        // ���������� ������� ������ 1 (��� �������� ������ �� �����)
        private void button1_Click(object sender, EventArgs e)
        {
            // ��������� ���� � ����� �� ���������� ���� richTextBox1
            string filePath = richTextBox1.Text.Trim();

            // ���������, ���������� �� ���� �� ���������� ����
            if (File.Exists(filePath))
            {
                try
                {
                    // ������ ���������� ����� � ������� ��� � richTextBox2
                    string fileContent = File.ReadAllText(filePath);
                    richTextBox2.Text = fileContent; // ���������� ���������� ����� � richTextBox2
                }
                catch (Exception ex)
                {
                    // ���� ��������� ������ ��� ������ �����, ������� ��������� �� ������
                    MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // ���� ���� �� ������, ������� ��������������
                MessageBox.Show("The specified file was not found. Check the path and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ���������� ������� ������ 2 (��� ������� �����)
        private void button2_Click(object sender, EventArgs e)
        {
            // ��������� �����, ���������� �� richTextBox2, �� ������ �� ������� ����� ������
            string[] lines = richTextBox2.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            // ��������� ���������� ������ �� ������ ������
            n = int.Parse(lines[0].Trim());

            // �������������� ������� ��������� �������� n x n
            adjacencyMatrix = new int[n, n];

            // ��������� ������� ���������
            for (int i = 0; i < n; i++) // �������� �� ���� ������� �������
            {
                // ��������� ������ �� ��������� ��������, ������� ������������ ����� �������� � ������ �������
                string[] row = lines[i + 1].Trim().Split(' ');

                for (int j = 0; j < n; j++) // �������� �� ������� �������� ������
                {
                    adjacencyMatrix[i, j] = int.Parse(row[j]); // ��������� ������� ��������� ���������� �� ������
                }
            }

            // ������ ��� �������� ������ ������ �����
            bool[] color = new bool[n];
            // ������ ��� ������������, ���� �� ������� ��������
            bool[] visited = new bool[n];
            // ���� ��� ��������, �������� �� ���� ����������
            bool isBipartite = true;

            // ������� ������ ������ ��� ����� �����
            setA.Clear();
            setB.Clear();

            // ���������� ������� ��� ���������� BFS
            Queue<int> queue = new Queue<int>();

            // �������� �� ���� �������� �����
            for (int i = 0; i < n; i++)
            {
                if (!visited[i]) // ���� ������� ��� �� ���� ��������
                {
                    // ��������� ������� � ������� ��� ������
                    queue.Enqueue(i);

                    // �������� � ������� ����� ��� ������� �������
                    color[i] = true;
                    visited[i] = true; // �������� ������� ��� ����������
                    setA.Add(i + 1); // ��������� ������� � ������ ���� (��������� � 1)

                    // BFS-����� �����
                    while (queue.Count > 0) // ���� � ������� ���� ��������
                    {
                        int vertex = queue.Dequeue(); // ��������� ������� �� �������

                        // �������� �� ���� ������� �������� �������
                        for (int j = 0; j < n; j++)
                        {
                            if (adjacencyMatrix[vertex, j] == 1) // ���� ���� ����� ����� ��������� vertex � j
                            {
                                if (!visited[j]) // ���� ������� j ��� �� ��������
                                {
                                    visited[j] = true; // �������� ������� ��� ����������
                                    color[j] = !color[vertex]; // ������ ���� �������
                                    queue.Enqueue(j); // ��������� ������� � �������

                                    // � ����������� �� ����� ��������� ������� � ���� �� �����
                                    if (color[j]) setA.Add(j + 1); // ���� ���� true, ��������� � setA (������ ����)
                                    else setB.Add(j + 1); // ���� ���� false, ��������� � setB (������ ����)
                                }
                                else if (color[j] == color[vertex]) // ���� ������� ������� ������ �����, ���� �� ����������
                                {
                                    isBipartite = false; // ���� �� ����������
                                }
                            }
                        }
                    }
                }
            }

            // ��������� ������ � richTextBox2 ��� �������
            richTextBox2.Text += "\n\n";

            // �������� ��� ������� ����� (��������� � 1)
            for (int i = 0; i < n; i++)
            {
                richTextBox2.Text += (i + 1) + " "; // ��������� ����� ������� � �����
            }

            richTextBox2.Text += "\n\n"; // ��������� ����� ������

            // �������� ������� ������ � ������ ���� � �����
            if (isBipartite) // ���� ���� ����������
            {
                richTextBox2.Text += "���� ����������\n\n"; // �����, ��� ���� ����������
                richTextBox2.Text += "������� ������ ����: " + string.Join(" ", setA) + "\n"; // ������� ������� ������ ����
                richTextBox2.Text += "������� ������ ����: " + string.Join(" ", setB) + "\n"; // ������� ������� ������ ����
                richTextBox2.Text += "\n";

                // ���� ������������� (�������� ��� ������ �������������)
                int[] match = new int[n];
                Array.Fill(match, -1); // ��������� ������ ������������� ��������� -1, ��� �������� ���������� ����

                // ���������� �������� ������ �������������
                foreach (int vertex in setA)
                {
                    bool[] visitedMatch = new bool[n]; // ������ ��� ������������ ���������� ������ ��� ������ ����
                    if (FindMatch(vertex - 1, match, visitedMatch)) // ���� ������������� ��� �������
                    {
                        richTextBox2.Text += (vertex) + " "; // ���� ����� ����, ������� �������
                    }
                }

                richTextBox2.Text += "\n"; // ��������� ����� ������ ��� �������������
                richTextBox2.Text += "\n�������������:\n"; // ��������� ��� �������������
                richTextBox2.Text += "\n";

                // �������� ����, ������� �������� �������������
                for (int i = 0; i < n; i++)
                {
                    if (match[i] != -1) // ���� ��� ������� ���� ����
                    {
                        richTextBox2.Text += (i + 1) + " - " + (match[i] + 1) + "\n"; // �������� ����
                    }
                }
            }
            else
            {
                richTextBox2.Text += "���� �� ����������\n"; // ���� ���� �� ����������, ������� ��������������� ���������
            }
        }

        // ����� ��� ������ �������������
        private bool FindMatch(int u, int[] match, bool[] visited)
        {
            // �������� �� ���� �������� ��� ������ ��������� ���� ��� ������� u
            for (int v = 0; v < n; v++)
            {
                if (adjacencyMatrix[u, v] == 1 && !visited[v]) // ���� ���������� ����� � ������� v �� ��������
                {
                    visited[v] = true; // �������� ������� ��� ����������

                    // ���� ������� v �� ����� ���� ��� ���� �� ����� �������������� ���� ��� ������� v
                    if (match[v] == -1 || FindMatch(match[v], match, visited))
                    {
                        match[v] = u; // ������������� ���� ��� ������� v
                        return true; // ����������, ��� ���� �������
                    }
                }
            }
            return false; // ���������� false, ���� ���� �� �������
        }

        // ���������� ������� ������ 3 (��� ������� ��������� �����)
        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = ""; // ������� ����� � richTextBox1
            richTextBox2.Text = ""; // ������� ����� � richTextBox2
        }
    }
}
