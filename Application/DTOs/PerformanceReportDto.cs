namespace Tarefando.Application.DTOs
{
    public class PerformanceReportDto
    {
        public int UserId { get; set; }
        public string UsuarioNome { get; set; } = string.Empty;
        public int TarefasConcluidas { get; set; }
        public double MediaTarefasPorDia { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
    }
}