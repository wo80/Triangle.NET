using System;
using System.Drawing;
using System.Windows.Forms;
using MeshExplorer.Generators;
using TriangleNet.Geometry;

namespace MeshExplorer
{
    public partial class FormGenerator : Form
    {
        public event EventHandler InputGenerated;

        IGenerator currentGenerator;

        public FormGenerator()
        {
            InitializeComponent();
        }

        private void UpdateControls()
        {
            if (currentGenerator.ParameterCount > 0)
            {
                sliderParam1.Enabled = true;
                lbParam1.Text = currentGenerator.ParameterDescription(0);
                lbParam1Val.Text = currentGenerator.ParameterDescription(0, sliderParam1.Value);
            }
            else
            {
                sliderParam1.Enabled = false;
                lbParam1.Text = "";
                lbParam1Val.Text = "";
            }

            if (currentGenerator.ParameterCount > 1)
            {
                sliderParam2.Enabled = true;
                lbParam2.Text = currentGenerator.ParameterDescription(1);
                lbParam2Val.Text = currentGenerator.ParameterDescription(1, sliderParam2.Value);
            }
            else
            {
                sliderParam2.Enabled = false;
                lbParam2.Text = "";
                lbParam2Val.Text = "";
            }

            if (currentGenerator.ParameterCount > 2)
            {
                sliderParam3.Enabled = true;
                lbParam3.Text = currentGenerator.ParameterDescription(2);
                lbParam3Val.Text = currentGenerator.ParameterDescription(2, sliderParam3.Value);
            }
            else
            {
                sliderParam3.Enabled = false;
                lbParam3.Text = "";
                lbParam3Val.Text = "";
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (currentGenerator != null && InputGenerated != null)
            {
                try
                {
                    var input = currentGenerator.Generate(sliderParam1.Value,
                        sliderParam2.Value, sliderParam3.Value);

                    InputGenerated(input, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    DarkMessageBox.Show("Exception", ex.Message);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
                this.Hide();
        }

        private void FormGenerator_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var rect = this.ClientRectangle;
            rect.Height -= 40;

            e.Graphics.FillRectangle(Brushes.DimGray, rect);
        }

        private void darkListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentGenerator = darkListBox1.SelectedItem as IGenerator;

            if (currentGenerator != null)
            {
                UpdateControls();
            }
        }

        private void sliderParam1_ValueChanging(object sender, EventArgs e)
        {
            if (currentGenerator != null)
            {
                lbParam1Val.Text = currentGenerator.ParameterDescription(0, sliderParam1.Value);
            }
        }

        private void sliderParam2_ValueChanging(object sender, EventArgs e)
        {
            if (currentGenerator != null)
            {
                lbParam2Val.Text = currentGenerator.ParameterDescription(1, sliderParam2.Value);
            }
        }

        private void sliderParam3_ValueChanging(object sender, EventArgs e)
        {
            if (currentGenerator != null)
            {
                lbParam3Val.Text = currentGenerator.ParameterDescription(2, sliderParam3.Value);
            }
        }

        private void FormGenerator_Load(object sender, EventArgs e)
        {
            darkListBox1.Items.Add(new RandomPoints());
            darkListBox1.Items.Add(new RandomPointsCircle());
            darkListBox1.Items.Add(new StarInBox());
            darkListBox1.Items.Add(new RingPolygon());
            darkListBox1.Items.Add(new BoxWithHole());
            darkListBox1.Items.Add(new CircleWithHole());

            darkListBox1.SelectedIndex = 0;
        }
    }
}
