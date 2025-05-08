using System.ComponentModel.DataAnnotations;

namespace RW.VAC.Application.Contracts.Opcs;

public class OpcItemCreateUpdateDto
{
    public Guid GroupId { get; set; }

    [Required(ErrorMessage = "请输入编码")]
    [MaxLength(50, ErrorMessage = "输入字符超过限制，最大长度50")]
    public string? Code { get; set; }

    [Required(ErrorMessage = "请输入标记名称")]
    [MaxLength(50, ErrorMessage = "输入字符超过限制，最大长度50")]
    public string? Name { get; set; }

    [MaxLength(100, ErrorMessage = "输入字符超过限制，最大长度50")]
    public string? Description { get; set; }

    public bool IsProcess { get; set; }
}