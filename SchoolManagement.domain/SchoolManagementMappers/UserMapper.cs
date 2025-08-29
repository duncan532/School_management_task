using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.domain.user;
using SchoolManagement.domain.SchoolManagement.dto;

namespace SchoolManagement.domain.SchoolManagementMappers
{
    public static class UserMapper
    {
        public static User ToEntity(this UserCreateDto dto)
        {
            return new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Age = dto.Age,
                Role = dto.Role,
                ClassroomId = dto.ClassroomId,
                Status = Status.Active,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };
        }
    }
}
