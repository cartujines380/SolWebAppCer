
using System;
using System.Configuration;

namespace wsProcesaPagosBG
{
    partial class ServProcesaPagos
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            string strIntervalo = ((string)ConfigurationManager.AppSettings["IntervaloTiempo"]).Trim();
            this.stLapso = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.stLapso)).BeginInit();
            // 
            // stLapso
            // 
            this.stLapso.Enabled = true;
            this.stLapso.Interval = Convert.ToDouble(strIntervalo);
            this.stLapso.Elapsed += new System.Timers.ElapsedEventHandler(this.stLapso_Elapsed);
            // 
            // ServProcesaPagos
            // 
            this.ServiceName = "Service1";
            ((System.ComponentModel.ISupportInitialize)(this.stLapso)).EndInit();

        }

        #endregion

        private System.Timers.Timer stLapso;
    }
}
