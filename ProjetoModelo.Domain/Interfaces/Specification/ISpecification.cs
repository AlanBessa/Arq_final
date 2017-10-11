﻿
namespace ProjetoModelo.Domain.Interfaces.Specification
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy(T entity);
    }
}
