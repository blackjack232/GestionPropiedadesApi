# 🏠 Technical Test – Fullstack Developer (Million Luxury)

Este proyecto es una solución para la **prueba técnica** de Million Luxury, desarrollada con **.NET 8**, **MongoDB** y **Next.js**.  
El objetivo es construir una API para gestionar propiedades y una interfaz web para consultarlas.

---

## 📌 Características

### Backend (API – .NET 8, C#)
- API REST para consultar propiedades desde MongoDB.
- Filtros por:
  - **Nombre** (`name`)
  - **Dirección** (`address`)
  - **Rango de precios** (`minPrice`, `maxPrice`)
- DTO con campos:
  - `IdOwner`
  - `Name`
  - `Address`
  - `Price`
  - **Solo una imagen principal**
- Arquitectura limpia con separación por capas.
- Manejo de errores centralizado.
- Unit tests con **NUnit**.

### Frontend (Next.js / React)
- Lista de propiedades obtenida desde la API.
- Filtros de búsqueda (nombre, dirección y rango de precios).
- Vista de detalles de cada propiedad.
- Diseño responsive.

---

## 🛠 Tecnologías Utilizadas

**Backend**
- .NET 8
- C#
- MongoDB Driver
- NUnit (testing)

**Frontend**
- Next.js
- React
- TailwindCSS (estilos)

**Base de Datos**
- MongoDB

