namespace GraphAnalisV2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int[,] adjacencyMatrix; // Матрица смежности
        private int n; // Число вершин
        private List<int> setA = new List<int>(); // Вершины первой доли
        private List<int> setB = new List<int>(); // Вершины второй доли

        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = richTextBox1.Text.Trim();

            if (File.Exists(filePath))
            {
                try
                {
                    string fileContent = File.ReadAllText(filePath);
                    richTextBox2.Text = fileContent;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The specified file was not found. Check the path and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] lines = richTextBox2.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            n = int.Parse(lines[0].Trim()); // Число вершин
            adjacencyMatrix = new int[n, n];

            // Заполняем матрицу смежности
            for (int i = 0; i < n; i++)
            {
                string[] row = lines[i + 1].Trim().Split(' ');
                for (int j = 0; j < n; j++)
                {
                    adjacencyMatrix[i, j] = int.Parse(row[j]);
                }
            }

            // Анализируем, является ли граф двудольным
            bool[] color = new bool[n]; // Цвета вершин
            bool[] visited = new bool[n];
            bool isBipartite = true;

            setA.Clear();
            setB.Clear();

            // Используем BFS для раскраски графа
            Queue<int> queue = new Queue<int>();
            for (int i = 0; i < n; i++)
            {
                if (!visited[i])
                {
                    queue.Enqueue(i);
                    color[i] = true; // Начинаем с первого цвета (true)
                    visited[i] = true;
                    setA.Add(i + 1); // Добавляем в первую долю (1-indexed)

                    while (queue.Count > 0)
                    {
                        int vertex = queue.Dequeue();

                        for (int j = 0; j < n; j++)
                        {
                            if (adjacencyMatrix[vertex, j] == 1) // Если есть ребро
                            {
                                if (!visited[j])
                                {
                                    visited[j] = true;
                                    color[j] = !color[vertex]; // Меняем цвет
                                    queue.Enqueue(j);

                                    if (color[j]) setA.Add(j + 1); // Вторая доля
                                    else setB.Add(j + 1); // Первая доля
                                }
                                else if (color[j] == color[vertex]) // Если ребра соединяют вершины одного цвета
                                {
                                    isBipartite = false;
                                }
                            }
                        }
                    }
                }
            }

            // Выводим результат
            richTextBox2.Text += "\n\n";

            // Печатаем числа вершин
            for (int i = 0; i < n; i++)
            {
                richTextBox2.Text += (i + 1) + " ";
            }

            richTextBox2.Text += "\n\n";

            // Печатаем раскрашенные множества
            if (isBipartite)
            {
                richTextBox2.Text += "Граф двудольный\n\n";
                richTextBox2.Text += "Вершины первой доли: " + string.Join(" ", setA) + "\n";
                richTextBox2.Text += "Вершины второй доли: " + string.Join(" ", setB) + "\n";
                richTextBox2.Text += "\n";
                // Поиск паросочетания
                int[] match = new int[n];
                Array.Fill(match, -1); // -1 означает, что вершина не имеет пары

                // Простейший алгоритм поиска паросочетания
                foreach (int vertex in setA)
                {
                    bool[] visitedMatch = new bool[n];
                    if (FindMatch(vertex - 1, match, visitedMatch))
                    {
                        richTextBox2.Text += (vertex) + " ";
                    }
                }

                // Печать паросочетания
                richTextBox2.Text += "\n";
                richTextBox2.Text += "\nПаросочетание:\n";
                richTextBox2.Text += "\n";
                for (int i = 0; i < n; i++)
                {
                    if (match[i] != -1)
                    {
                        richTextBox2.Text += (i + 1) + " - " + (match[i] + 1) + "\n";
                    }
                }
            }
            else
            {
                richTextBox2.Text += "Граф не двудольный\n";
            }
        }

        // Метод для поиска паросочетания
        private bool FindMatch(int u, int[] match, bool[] visited)
        {
            for (int v = 0; v < n; v++)
            {
                if (adjacencyMatrix[u, v] == 1 && !visited[v])
                {
                    visited[v] = true;

                    if (match[v] == -1 || FindMatch(match[v], match, visited))
                    {
                        match[v] = u;
                        return true;
                    }
                }
            }
            return false;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox2.Text = "";
        }
    }
}
