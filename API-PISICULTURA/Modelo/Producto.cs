namespace API.Modelo
{
    public class Gasto
    {
        public int GastoID { get; set; }
        public string FechaGasto { get; set; }
        public string TipoGasto { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Notas { get; set; }
    }
}
