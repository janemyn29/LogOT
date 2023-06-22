
using FluentValidation;

namespace mentor_v1.Application.MaternityEmployee.Commands.UpdateMaternityEmployee;
public class UpdateMaternityEmployeeValidator : AbstractValidator<UpdateMaternityEmployeeViewModel>
{
    public UpdateMaternityEmployeeValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Id không được để trống.");

        RuleFor(v => v.Image).NotEmpty().WithMessage("Hình ảnh không được để trống");

        RuleFor(v => v.BirthDay).NotEmpty().WithMessage("Ngày sinh không được để trống.");

        RuleFor(v => v.AcceptanceType).NotNull().WithMessage("Loại chập nhận không được để trống.").
            GreaterThan(0).WithMessage("Loại chập nhận phải lớn hơn 0").LessThan(4)
            .WithMessage("Loại chập nhận phải bé hơn 4.");

        RuleFor(x => x.DenyReason).NotEmpty().WithMessage("Lý do từ chối không được để trống");
    }
}