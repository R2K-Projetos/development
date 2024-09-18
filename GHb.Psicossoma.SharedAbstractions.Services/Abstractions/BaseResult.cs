namespace Ghb.Psicossoma.SharedAbstractions.Services.Abstractions;

/// <summary>
/// Objeto mantenedor de resultado padrão para as chamadas das APIs do projeto
/// </summary>
/// <typeparam name="T">Tipo de objeto carregado no resultado</typeparam>
public abstract class BaseResult<T> where T : class
{

    /// <summary>
    /// Retorna uma lista de items quando o resultado for uma lista
    /// </summary>
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

    /// <summary>
    /// Quando não exitir resultado a ser retornado, será apenas marcado para indicar que foi executado
    /// </summary>
    public bool? WasExecuted { get; set; }

    /// <summary>
    /// Identificador da página atual do resultado
    /// </summary>
    public int? CurrentPage { get; set; }

    /// <summary>
    /// Tamanho da página indicado na execução de uma pesquisa
    /// </summary>
    public int? PageSize { get; set; }

    /// <summary>
    /// Quantidade total de itens que estão no resultado da pesquisa
    /// </summary>
    public long? TotalItems { get; set; }

    /// <summary>
    /// Mensagem de erro
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Exceção gerada durante a execução
    /// </summary>
    public Exception? Exception { get; set; } = null;

    /// <summary>
    /// Indicativo de erro
    /// </summary>
    public bool HasError { get; set; }

    /// <summary>
    /// Código do erro
    /// </summary>
    public int ResponseCode { get; set; }

    /// <summary>
    /// Tempo decorrido para devolver a resposta da execução
    /// </summary>
    public TimeSpan ElapsedTime { get; set; }

    /// <summary>
    /// Quantidade total de páginas para o resultado da pesquisa
    /// </summary>
    public int? TotalPages
    {
        get
        {
            if (TotalItems == null || PageSize == null) return null;

            var result = (int)Math.Ceiling(((double)(TotalItems ?? 0) / (double)(PageSize ?? 1)));

            return result < 1 ? 1 : result;
        }
    }

    /// <summary>
    /// Indicador se existe uma página antes da página atual
    /// </summary>
    public bool? HasPreviousPage
    {
        get
        {
            if (CurrentPage == null) return null;

            return ((CurrentPage ?? 0) > 1);
        }
    }

    /// <summary>
    /// Indicador se existe uma página depois da página atual
    /// </summary>
    public bool? HasNextPage
    {
        get
        {
            if (CurrentPage == null || TotalPages == null) return null;

            return ((CurrentPage ?? 0) < (TotalPages ?? 0));
        }
    }

    /// <summary>
    /// Inicializa o conjunto de atributos de erro em uma única chamada
    /// </summary>
    public void BindError(int responseCode, string errorMessage, Exception? exception = null)
    {
        WasExecuted = false;
        HasError = true;
        ResponseCode = responseCode;
        Message = errorMessage;
        Exception = exception;
    }
}
