using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//
using System.Threading;
using System.Speech;
using System.Speech.Recognition;
using System.Speech.Synthesis;



namespace SpeechRecog
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public SpeechRecognitionEngine recognizer;
        public Grammar grammer;
        public Thread RecThread;
        public Boolean RecognizerState = true;

        private void Form1_Load(object sender, EventArgs e)
        {
            GrammarBuilder builder = new GrammarBuilder();
            builder.AppendDictation();
            grammer = new Grammar(builder);
            recognizer = new SpeechRecognitionEngine();
            recognizer.LoadGrammar(grammer);
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
            RecognizerState = true;
            RecThread = new Thread(new ThreadStart(RecThreadFunction));
            RecThread.Start();
        }


        public void RecThreadFunction()
        {
            while (true)
            {
                try
                {
                    recognizer.Recognize();
                }
                catch
                {
                }
            }
        }



        void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (!RecognizerState)

                return;

            this.Invoke((MethodInvoker)delegate
            {
                richTextBox1.Text += (" " + e.Result.Text.ToLower());
            });

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RecognizerState = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RecognizerState = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            RecThread.Abort();
            RecThread = null;
            recognizer.UnloadAllGrammars();
            recognizer.Dispose();
            grammer = null;
        }

        //



    }
}
