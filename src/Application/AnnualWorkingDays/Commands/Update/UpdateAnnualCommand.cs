﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.AnnualWorkingDays.Commands.Update;
public record UpdateAnnualCommand : IRequest
{
    public Guid Id { get; set; }
    public DateTime Day { get; set; }
    public bool IsHoliday { get; set; }
}

public class UpdateAnnualCommandHandler : IRequestHandler<UpdateAnnualCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateAnnualCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateAnnualCommand request, CancellationToken cancellationToken)
    {
        var current  = _context.Get<Domain.Entities.AnnualWorkingDay>().Where(x => x.Id == request.Id && x.IsDeleted == false).FirstOrDefault();

        if (current == null)
        {
            throw new NotFoundException("Không tìm thấy ngày mà bạn yêu cầu!");
        }
        var existDay = _context.Get<Domain.Entities.AnnualWorkingDay>().Where(x => x.Day.Date == request.Day.Date && x.IsDeleted == false).FirstOrDefault();

        if (existDay != null)
        {
            throw new NotFoundException("Ngày "+ request.Day.ToString("dd/MM/yyyy")+" đã tồn tại!");
        }
        var existedDay = _context.AnnualWorkingDays.Where(x => x.IsDeleted == false && x.Day.Date == request.Day.Date).FirstOrDefault();
        if (existedDay != null)
        {
            throw new InvalidDataException("Ngày " + request.Day.ToString("dd/MM/yyyy") + " đã tồn tại!");
        }
        else if (DateTime.Now.Date > request.Day.Date)
        {
            throw new InvalidDataException("Ngày được thêm vào phải lớn hơn hoặc nhỏ hơn ngày hiện tại! ");

        }
        try
        {
            TypeDate typeDate;
            ShiftType shiftType;
            Guid coeId;
            if (request.Day.DayOfWeek == DayOfWeek.Saturday)
            {
                shiftType = _context.ConfigDays.FirstOrDefault().Saturday;
                typeDate = TypeDate.Saturday;
                coeId = _context.Coefficients.Where(x => x.TypeDate == typeDate).FirstOrDefault().Id;
                current.CoefficientId = coeId;
                    current.TypeDate = typeDate;
                    current.ShiftType = shiftType;
                current.Day = request.Day;
            }
            else if (request.Day.DayOfWeek == DayOfWeek.Sunday)
            {
                shiftType = _context.ConfigDays.FirstOrDefault().Sunday;
                typeDate = TypeDate.Sunday;
                coeId = _context.Coefficients.Where(x => x.TypeDate == typeDate).FirstOrDefault().Id;
                current.Day = request.Day;
                current.CoefficientId = coeId;
                current.TypeDate = typeDate;
                current.ShiftType = shiftType;
            }
            else if (request.IsHoliday)
            {

                shiftType = _context.ConfigDays.FirstOrDefault().Holiday;
                typeDate = TypeDate.Holiday;
                coeId = _context.Coefficients.Where(x => x.TypeDate == typeDate).FirstOrDefault().Id;
                current.Day = request.Day;
                current.CoefficientId = coeId;
                current.TypeDate = typeDate;
                current.ShiftType = shiftType;
            }else
            {
                shiftType = _context.ConfigDays.FirstOrDefault().Normal;
                typeDate = TypeDate.Normal;
                coeId = _context.Coefficients.Where(x => x.TypeDate == typeDate).FirstOrDefault().Id;
                current.Day = request.Day;
                current.CoefficientId = coeId;
                current.TypeDate = typeDate;
                current.ShiftType = shiftType;
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
        catch (Exception ex)
        {
            throw new Exception("Đã xảy ra lỗi trong quá trình cập nhật!");
        }
        
    }
}