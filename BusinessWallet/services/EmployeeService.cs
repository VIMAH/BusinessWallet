using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessWallet.DTOs;
using BusinessWallet.models;
using BusinessWallet.models.Enums;
using BusinessWallet.repository;

namespace BusinessWallet.services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;
        public EmployeeService(IEmployeeRepository repo) => _repo = repo;

        public async Task<EmployeeReadDto> CreateAsync(EmployeeCreateDto dto)
        {
            var e = Map(dto);
            var saved = await _repo.CreateAsync(e);
            return ToReadDto(saved);
        }

        public async Task<EmployeeReadDto?> GetByIdAsync(Guid id) =>
            (await _repo.GetByIdAsync(id)) is { } e ? ToReadDto(e) : null;

        public async Task<IEnumerable<EmployeeReadDto>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(ToReadDto);

        public async Task<EmployeeReadDto?> UpdateAsync(Guid id, EmployeeUpdateDto dto)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e is null) return null;

            Patch(e, dto);
            await _repo.UpdateAsync(e);
            return ToReadDto(e);
        }

        public Task<bool> DeleteAsync(Guid id) => _repo.DeleteAsync(id);

        // ───────────────────────── helpers
        private static Employee Map(EmployeeCreateDto d) => new()
        {
            FirstName = d.FirstName,
            LastName = d.LastName,
            Voorvoegsel = d.Voorvoegsel,
            Voorletters = d.Voorletters,
            BirthName = d.BirthName,
            Gender = d.Gender,
            BirthDate = d.BirthDate,
            OlderThan18 = d.OlderThan18 ?? false,
            BirthPlace = d.BirthPlace,
            BirthCountry = d.BirthCountry,
            Married = d.Married ?? false,
            LegalName = d.LegalName,
            Category = d.Category,
            City = d.City,
            Kvk = d.Kvk,
            Position = d.Position,
            Email = d.Email,
            PhoneNumber = d.PhoneNumber,
            VerificationState = d.VerificationState ?? VerificationState.Unverified,
            EmployeeState = d.EmployeeState ?? EmployeeState.Inactive,
            PublicKey = d.PublicKey,
            LaatsteAanmelding = d.LaatsteAanmelding
        };

        private static void Patch(Employee e, EmployeeUpdateDto d)
        {
            if (d.FirstName != null) e.FirstName = d.FirstName;
            if (d.LastName != null) e.LastName = d.LastName;
            if (d.Voorvoegsel != null) e.Voorvoegsel = d.Voorvoegsel;
            if (d.Voorletters != null) e.Voorletters = d.Voorletters;
            if (d.BirthName != null) e.BirthName = d.BirthName;
            if (d.Gender != null) e.Gender = d.Gender;
            if (d.BirthDate.HasValue) e.BirthDate = d.BirthDate;
            if (d.OlderThan18.HasValue) e.OlderThan18 = d.OlderThan18.Value;
            if (d.BirthPlace != null) e.BirthPlace = d.BirthPlace;
            if (d.BirthCountry != null) e.BirthCountry = d.BirthCountry;
            if (d.Married.HasValue) e.Married = d.Married.Value;
            if (d.LegalName != null) e.LegalName = d.LegalName;
            if (d.Category != null) e.Category = d.Category;
            if (d.City != null) e.City = d.City;
            if (d.Kvk != null) e.Kvk = d.Kvk;
            if (d.Position != null) e.Position = d.Position;
            if (d.Email != null) e.Email = d.Email;
            if (d.PhoneNumber != null) e.PhoneNumber = d.PhoneNumber;
            if (d.VerificationState.HasValue) e.VerificationState = d.VerificationState.Value;
            if (d.EmployeeState.HasValue) e.EmployeeState = d.EmployeeState.Value;
            if (d.PublicKey != null) e.PublicKey = d.PublicKey;
            if (d.LaatsteAanmelding.HasValue) e.LaatsteAanmelding = d.LaatsteAanmelding;
            e.UpdatedAt = DateTime.UtcNow;
        }

        private static EmployeeReadDto ToReadDto(Employee e) => new()
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Voorvoegsel = e.Voorvoegsel,
            Voorletters = e.Voorletters,
            FullName = e.FullName,
            BirthName = e.BirthName,
            Gender = e.Gender,
            BirthDate = e.BirthDate,
            OlderThan18 = e.OlderThan18,
            BirthPlace = e.BirthPlace,
            BirthCountry = e.BirthCountry,
            Married = e.Married,
            LegalName = e.LegalName,
            Category = e.Category,
            City = e.City,
            Kvk = e.Kvk,
            Position = e.Position,
            Email = e.Email,
            PhoneNumber = e.PhoneNumber,
            VerificationState = e.VerificationState,
            EmployeeState = e.EmployeeState,
            PublicKey = e.PublicKey,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt,
            LaatsteAanmelding = e.LaatsteAanmelding
        };
    }
}
