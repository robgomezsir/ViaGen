# VIA:GEN — Interface e Experiência do Jogador

## Filosofia
Minimalista.
Imersiva.
Diegética.

---

# Painel de Jogo

## Inferior esquerdo
- oxigênio
- energia
- temperatura

## Inferior direito
- ferramenta equipada
- slots de acesso rápido

## Escâner
Interface holográfica futurista.

## Memórias
Durante memórias:
- painel de jogo desaparece
- tela distorce
- áudio fica abafado

---

# Menu principal

## Fundo
- Imagem estática `menu.png` (`Resources/Art/UI/Menu/MenuBackdrop`)
- Sem parallax, sem animação na arte de fundo

## Seleção (animado)
- 6 itens: Continuar, Novo Destino, Memórias, Nave, Opções, Sair
- Hover: glow ciano, deslocamento de texto, escala do ícone
- Entrada: stagger + fade (`MainMenuAnimator`)
- Rodapé: citação + waveform (`MenuAudioVisualizer`)

## Fluxo de cenas
`Bootstrap` → `MainMenu` → planetas / nave
