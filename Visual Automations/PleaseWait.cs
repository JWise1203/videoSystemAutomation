using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Visual_Automations
{
    public partial class PleaseWait : Form
    {
        public Action Worker { get; set; }

        public PleaseWait(Action worker)
        {
            InitializeComponent();

            if (worker == null)
                throw new ArgumentNullException();

            Worker = worker;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Task.Factory.StartNew(Worker).ContinueWith(t => { this.Close(); }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    
    }
}
