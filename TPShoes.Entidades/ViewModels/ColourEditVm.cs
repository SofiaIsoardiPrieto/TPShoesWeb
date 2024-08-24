﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPShoes.Entidades.ViewModels
{
	public class ColourEditVm
	{
		public int ColourId { get; set; }
		[Required(ErrorMessage = "{0} es requerido")]
		[StringLength(50, ErrorMessage = "{0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
		[DisplayName("Nombre Colour")]
		public string ColourName { get; set; } = null!;


	}
}