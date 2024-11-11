using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneradorLicencias
{
    public partial class frmMain : Form
    {
        private readonly Licencia Licencia;
        private int pagina = 1;
        private int total = 0;
        private int pageSize = 20;
        private double totalPages = 0;
        string filter = "";
        bool cargando = true;

        private List<ClientesViewModel> dataCli = new List<ClientesViewModel>();
        public frmMain()
        {
            InitializeComponent();

            Licencia = new Licencia();

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            cargando = true;
            filter = txtBuscar.Text;
            pagina = 1;
            DataSource(filter);
            cargando = false;
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            pagina = 1;
            DataSource();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            pagina--;
            if (pagina <= 0)
                pagina = 1;
            DataSource(filter);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            pagina++;

            if (pagina > totalPages)
                pagina = System.Convert.ToInt32(totalPages);

            DataSource(filter);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {

            pagina = System.Convert.ToInt32(totalPages);

            DataSource(filter);
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {

            Licencia.Plan = cmbPlan.SelectedValue.ToString();
            Licencia.Cliente = cmbCliente.SelectedValue.ToString();
            Licencia.CantidadProveedores = int.Parse(txtCantMax.Value.ToString());

            var cod = Licencia.GenerarLicencia();

            txtLicencia.Text = cod.ToString();

            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var info = Licencia.DesencriptarLicencia(txtLicencia.Text);
            var arr_info = info.Split('-');


            DAL.InsertLicencia(new Licencias
            {
                 Id =int.Parse( (string.IsNullOrEmpty(lblLicId.Text) ? "0" : lblLicId.Text)),
                ClienteId = int.Parse(arr_info[0]),
                PlanId = int.Parse(arr_info[1]),
                Licencia = txtLicencia.Text,
                Activo = true,
                cantidadMaxProveedores = int.Parse(arr_info[2]),
                fechaCreacion = DateTime.Now,
                Vencimiento = DateTime.Now.AddYears(1),
            });


            DataSource();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            dgClientes.RowHeadersVisible = false;
            LlenarCombos();
            DataSource();
            cargando = false;
        }

        private void LlenarCombos()
        {
            cmbCliente.ValueMember = nameof(Clientes.Id);
            cmbCliente.DisplayMember = nameof(Clientes.proveedor);
            cmbCliente.DataSource = DAL.ClientesList();
            cmbCliente.SelectedIndex = 0;

            cmbPlan.ValueMember = nameof(Plan.Id);
            cmbPlan.DisplayMember = nameof(Plan.plan);
            cmbPlan.DataSource = DAL.PlanesList();
            cmbPlan.SelectedIndex = 0;

        }

        private void DataSource(string filter = "")
        {

            List<ClientesViewModel> data = default;

            dataCli = DAL.ClientesInfo();

            if (filter != "")
            {
                data = dataCli.Where(x => x.Cliente.ToLower().Contains(filter.ToLower())).ToList();
            }
            else
            {
                data = dataCli;
            }

            total = data.Count();
            totalPages = total / pageSize;

            if ((total % pageSize) != 0)
                totalPages = Math.Round(totalPages, 0) + 1;

            var dataPage = data.Skip(pageSize * (pagina - 1)).Take(pageSize).ToList();

            txtPagina.Text = $"pagina {pagina} / {totalPages} de {total} registros";
            dgClientes.DataSource = dataPage;



        }

        private void cmbPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if(cmbPlan.SelectedValue == null)   return;

            var id = cmbPlan.SelectedValue.ToString();

             var cantidad = DAL.PlanesList().FirstOrDefault(x => x.Id == int.Parse(id) ).cantidad;

            txtCantMax.Value = cantidad;

        }

        private void dgClientes_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (cargando)
                return;

            var id = dgClientes["Id", e.RowIndex].Value;
            var codigo = dgClientes["ClienteId", e.RowIndex].Value ;
            var plan = dgClientes["PlanId", e.RowIndex].Value ;
            var licencia = dgClientes["Lic", e.RowIndex].Value;


            cmbCliente.SelectedValue =int.Parse( codigo.ToString());
            cmbPlan.SelectedValue = int.Parse(plan.ToString());
            lblLicId.Text = id.ToString();
            txtLicencia.Text = licencia.ToString();
            btnSave.Enabled = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = string.Empty;
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnBuscar_Click(sender, e);
            }
        }

        private void cmbCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtLicencia.Text = string.Empty;
            lblLicId.Text = string.Empty;
        }
    }
}
