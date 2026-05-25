using System.Collections.Generic;

namespace ViaGen.Core
{
    public static class Localization
    {
        private static readonly Dictionary<string, string> Pt = new()
        {
            ["menu_continue_title"] = "CONTINUAR",
            ["menu_continue_sub"] = "ÚLTIMA JORNADA",
            ["menu_new_title"] = "NOVO DESTINO",
            ["menu_new_sub"] = "INICIAR JORNADA",
            ["menu_memories_title"] = "MEMÓRIAS",
            ["menu_memories_sub"] = "REGISTROS RECUPERADOS",
            ["menu_ship_title"] = "NAVE",
            ["menu_ship_sub"] = "SISTEMAS E MELHORIAS",
            ["menu_options_title"] = "OPÇÕES",
            ["menu_options_sub"] = "CONFIGURAÇÕES",
            ["menu_quit_title"] = "SAIR",
            ["menu_quit_sub"] = "ENCERRAR SISTEMA",
            ["menu_quote"] = "SE ALGUÉM OUVIR ISSO... ESTOU VOLTANDO PARA CASA.",
            ["menu_subtitle"] = "ECHOES OF EXODUS",
            ["menu_version"] = "VIA:GEN // v1.0.0",
            ["menu_online"] = "SISTEMA ONLINE",
            ["menu_coming_soon"] = "EM BREVE",
            ["menu_new"] = "Novo jogo",
            ["menu_continue"] = "Continuar",
            ["menu_quit"] = "Sair"
        };

        public static string Get(string key) =>
            Pt.TryGetValue(key, out var value) ? value : key;
    }
}
