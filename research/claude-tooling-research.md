# Claude Tooling Research — MCP, Plugin, Subagent Ekosistemi (Nisan 2026)

**Amaç**: MobileGameFactory için Claude Code ekosisteminden hangi MCP sunucularının, plugin'lerin ve subagent pattern'larının üretim hattına girebileceğini haritalamak. Kaynak taraması: Anthropic docs, modelcontextprotocol.io, awesome-mcp-servers, PulseMCP, GitHub.

---

## 1. Model Context Protocol (MCP) — Kısa Durum

MCP, Anthropic'in Kasım 2024'te açtığı açık standart; AI client ile harici araç/veri arasındaki köprü. 2026 itibariyle:

- **Registry**: `modelcontextprotocol/servers` (resmi), `mcp.so`, `pulsemcp.com`, `glama.ai/mcp`, `mcpservers.org`.
- **Awesome listeler**: `punkpeye/awesome-mcp-servers`, `appcypher/...`, `wong2/...`, `TensorBlock/awesome-mcp-servers` (7200+ sunucu).
- Transport: stdio (lokal), HTTP/SSE (remote). Claude Code her ikisini de destekler.

Bizim için kritik olan: MCP sunucusu ekledikçe agent'lar dosya yazmak yerine **tool çağırır** — token ekonomisi kuralımızın tam karşılığı.

---

## 2. Claude Code Plugin Sistemi (2026)

Ekim 2025 sonrası resmi **plugin** sistemi aktif. Bir plugin şunları paketler:

- **Skills** (`.claude/skills/<ad>/SKILL.md`) — slash-command ile çağrılan playbook'lar.
- **Subagents** (`.claude/agents/*.md`) — YAML frontmatter: `tools`, `model`, `mcpServers`, `hooks`, `permissionMode`, `maxTurns`.
- **Hooks** (`command | http | prompt | agent`) — PreToolUse, PostToolUse gibi event'lere bağlanır.
- **Slash commands** (`.claude/commands/*.md`).
- **MCP server bağlantıları** (plugin içinden).

Marketplace'ler:
- `anthropics/claude-plugins-official` (33 resmi plugin — language server, dev workflow, setup).
- `ComposioHQ/awesome-claude-plugins` (community index).
- `claudemarketplaces.com`, `buildwithclaude.com`, `aitmpl.com/plugins`.
- Kurulum: `/plugin install <ad>@<marketplace>`.

Mart 2026'da ~101 plugin aktif. Bizim repo zaten plugin şemasına uygun (`.claude/agents/`, `.claude/skills/`, `.claude/rules/`), sadece bir `marketplace.json` eklersek dahili plugin olarak dağıtılabilir.

**Şubat 2026 eklemesi: Agent Teams** — subagent YAML'dan bağımsız, prompt'tan tanımlanan çoklu lead/worker oturumları. Opus 4.6 ile geldi. Paralel orchestrasyon için alternatif yol.

---

## 3. Oyun Pipeline'ı için MCP Sunucuları

### 3.1 Oyun motoru (referans, biz MAUI kullanıyoruz)
| Ad | Ne yapar | Link | Kurulum | Fayda |
|---|---|---|---|---|
| mcp-unity | Unity Editor bridge (scene, asset, script) | `CoderGamester/mcp-unity` | Orta | Bizim için doğrudan yok; MAUI muadili yok, ama pattern referansı |
| Unity MCP (Coplay) | Sahne + asset yönetimi | `CoplayDev/unity-mcp` | Orta | Aynı |
| Godot MCP | Godot editor kontrolü | `Coding-Solo/godot-mcp`, `bradypp/godot-mcp` | Orta | Aynı |

**Tavsiye**: MAUI için bir **custom MCP** yazmayı değerlendirilebilir (GraphicsView/Skia scene inspector). Şimdilik gerek yok.

### 3.2 Tasarım & UI
| Ad | Ne yapar | Link | Kurulum | Fayda |
|---|---|---|---|---|
| **Figma MCP** | Figma design → kod + design context | `help.figma.com/.../figma-mcp-server` (resmi) | Kolay | UI iskelet export, icon/screenshot alma. Game Designer ve Store/Release için altın |

### 3.3 Asset üretimi (2D/3D/ses)
| Ad | Ne yapar | Link | Kurulum | Fayda |
|---|---|---|---|---|
| **Ludo.ai MCP** | Üretim-hazır sprite + spritesheet + ses seti + müzik, tek pipeline | `pulsemcp.com/servers/game-assets` | Kolay (API key) | Oyun asset pipeline'ı için en bütünsel. Mart 2026 beta |
| Game Asset Generator MCP | HF Spaces üzerinden 2D/3D (OBJ/GLB) asset | `MubarakHAlketbi/game-asset-mcp` | Orta | Ücretsiz HF kullanımı |
| ComfyMCP Studio | `create_sprite_atlas`, spritesheet + Unity export | `glama.ai/.../ComfyAI-MCP-GameAssets` | Zor (ComfyUI gerekir) | Sprite atlas otomasyonu |
| mcp-server-stability-ai | SD 3.5 generate/edit/upscale | `tadasant/mcp-server-stability-ai` | Kolay | Background, ikon, store screenshot |
| Replicate Image Gen MCP | Replicate API köprüsü (FLUX, SDXL, vs.) | `mcp.aibase.com/server/1916341309847347202` | Kolay | Esnek model seçimi |
| mcp-image (Gemini Nano Banana) | Auto-optimize prompt + quality presets | `shinpr/mcp-image` | Kolay | Hızlı placeholder |
| Scenario.com MCP | Style-consistent game art | Scenario bridge | Orta | Stil tutarlılığı (brand art) |
| Tripo 3D MCP | Text → 3D model + Blender bridge | `pulsemcp.com/servers/vast-ai-tripo-3d` | Orta | 3D gerekirse |
| Meshy MCP | Text-to-3D, PBR texture, image-to-3D | `fabianh001-threedee-meshy` | Orta | Aynı |
| **Sound Effects MCP** | SFX üretim + coding event sesleri | `pulsemcp.com/servers/sound-effects` | Kolay | Oyun SFX + dev feedback |

Font library için ayrı MCP yok; Google Fonts API üzerinden basit custom MCP yazılabilir.

### 3.4 Backend / Analytics (local-first kuralımıza uygun olanlar)
| Ad | Ne yapar | Link | Kurulum | Fayda |
|---|---|---|---|---|
| **Firebase MCP** (resmi) | Firebase proje/app create, Crashlytics crash özeti, SDK config | `firebase.google.com/docs/ai-assistance/mcp-server` | Kolay | Crashlytics bizim için değerli (local log + opt-in crash) |
| firebase-mcp (community) | Full Firebase admin | `gannonh/firebase-mcp` | Orta | Gerekmedikçe atla |
| Mixpanel MCP (hosted) | Events, funnel, retention NL sorgular | `docs.mixpanel.com/docs/mcp` | Kolay | Post-launch analiz |
| Amplitude MCP | Amplitude API | `silviorodrigues/amplitude-mcp` | Kolay | Alternatif |
| Google Developer Knowledge MCP | Android/Firebase/Play doc arama | `developers.google.com/knowledge/mcp` | Kolay | MAUI Developer için docs lookup |

Anayasamızda "backend yok, sync yok" kuralı var — Firebase'i **sadece** Crashlytics + anonim analytics için kullanabiliriz; veri depolama kalıyor SQLite.

### 3.5 Store Automation (ship kapısı için kritik)
| Ad | Ne yapar | Link | Kurulum | Fayda |
|---|---|---|---|---|
| **play-store-mcp** | AAB upload, track promote, release status | `devexpert-io/play-store-mcp` | Orta (service account) | Store/Release agent'ın core aracı |
| google-play-mcp (Python) | Full release lifecycle + 2026 battery health compliance monitoring | `mcpservers.org/.../mcp-google-play` | Orta | Android Vitals + yeni Play kuralları |
| Google Play Console MCP | AgiMaulana implementation | `glama.ai/.../GooglePlayConsoleMcp` | Orta | Alternatif |
| Unified App Store Connect + Play MCP | Listing, screenshot, release, review yanıtları | `app-publish-mcp` (glama) | Zor (iki credential) | Tek noktadan iki store |

### 3.6 CI/CD
| Ad | Ne yapar | Link | Kurulum | Fayda |
|---|---|---|---|---|
| **Bitrise MCP (resmi)** | Build log debug, workflow optimize, trigger build | `docs.bitrise.io/.../bitrise-mcp` | Kolay | Mobile-native CI; Windows'tan iOS archive için zorunlu yollardan biri |
| Codemagic | Native MCP yok 2026-04; REST API sarılabilir | `codemagic.io` | Orta (custom) | Flutter-odaklı ama MAUI yok |
| GitHub MCP (resmi) | PR, issue, Actions tetik | `github/github-mcp-server` | Kolay | Her agent için temel |

Fastlane'i doğrudan MCP yok — Bitrise/Codemagic içinde fastlane lane çağrısı pattern'i standart.

### 3.7 Veri / Altyapı (bonus)
- **SQLite MCP** (`modelcontextprotocol/servers` içinde) — Bizim `ops/factory.db`'ye ek köprü olabilir; zaten kendi MCP'miz var.
- **Filesystem, Git, Memory** — resmi reference server'lar.

---

## 4. Subagent & Agent SDK Pattern'ları

2026 Claude Agent SDK desenleri:

- **Split-and-merge**: Bağımsız altgörevler paralel koşar, sonra birleştirir. Bizim gate workflow'umuz için: Market + Design yazarken Monetization ilk taslağını paralel hazırlayabilir (bağımsızsa).
- **Operator pattern**: Tek bir orchestrator (bizde PM) delegate eder, sonuçları toplar. Şu an tam bunu yapıyoruz.
- **Domain routing**: Her subagent kendi dosya alanına dokunur — çakışma yok. Bizde zaten `games/<id>/<artifact>.md` ayrımı var, doğru yoldayız.
- **Agent Teams (Şubat 2026)**: Prompt-only tanımlı çoklu oturum; YAML'sız. Orchestrasyonu Claude kendi yönetir. Bizim PM pattern'ımızın yerini alabilir ama kontrolü kaybederiz — **benimsenmemeli**.
- **Performans**: Paralel subagent → total time ≈ en yavaş task. Research-heavy işlerde 3-4x speedup raporlanıyor.

Önerilen kural: "Paralel yalnızca farklı dosya alanlarında çalışılıyorsa." Bu zaten CLAUDE.md'de "PM bağımsız kapıları paralel çalıştırır" diye kayıtlı; SDK pattern'ı ile hizalı.

---

## 5. Workflow Otomasyonu Pattern'ları

- **Hooks**: PreToolUse ile `log_append` zorunluluğu kontrolü, PostToolUse ile artifact_register tetikleme. Bizim "chatter log değil karar log" kuralımız PreToolUse command hook ile enforce edilebilir.
- **Skills 2.0** (Nisan 2026): allowed-tools, model override, agent routing, nested hooks frontmatter'da. Bizim mevcut skill dosyalarımız bu şemaya uygun — güncel.

---

## Özet (≤200 kelime)

**MobileGameFactory'e eklenmesi öncelikli 5 MCP sunucusu**:

1. **Ludo.ai MCP** — sprite + spritesheet + koordineli SFX tek çağrıda; asset pipeline'ımızın omurgası. MAUI Developer + Game Designer için.
2. **Figma MCP** (resmi) — ikon/screenshot/UI mockup export; Store/Release ve Designer için kritik.
3. **mcp-server-stability-ai** veya **Replicate Image Gen MCP** — store screenshot, feature graphic, placeholder art. Kolay, ucuz, test edilmiş.
4. **play-store-mcp** — Store/Release agent'ın şu an manuel yaptığı Play Console adımlarını otomatikleştirir; Android-first stratejimizin gerçek kaldıracı.
5. **Bitrise MCP (resmi)** — Windows'tan iOS build + crash/log teşhis; "Mac bulunca iOS" kuralını Bitrise cloud Mac ile kırabilir.

**Agent SDK tavsiyesi**: Operator pattern'ı (PM + 7 worker) koru. Agent Teams'e geçme — orchestrasyon kontrolünü kaybedersin, bizim gate disiplinimizle uyumsuz. Split-and-merge'ü PM'in paralel kapı tetiklemesinde kullan (Market + erken Design brief eş zamanlı olabilir). Skill'leri Skills 2.0 frontmatter'ına migrate et (`allowed-tools`, `model`). Chatter-vs-karar disiplinini **PreToolUse hook** ile enforce et — `log_append` dışı yazmalarda uyarı. Repo'yu `marketplace.json` ile internal plugin olarak paketle; gelecekte başka sahiplerle paylaşılabilir.

---

## Kaynaklar

- https://code.claude.com/docs/en/plugin-marketplaces
- https://github.com/anthropics/claude-plugins-official
- https://github.com/ComposioHQ/awesome-claude-plugins
- https://github.com/punkpeye/awesome-mcp-servers
- https://github.com/TensorBlock/awesome-mcp-servers
- https://github.com/modelcontextprotocol/servers
- https://platform.claude.com/docs/en/agent-sdk/subagents
- https://platform.claude.com/docs/en/agent-sdk/overview
- https://code.claude.com/docs/en/sub-agents
- https://code.claude.com/docs/en/hooks
- https://platform.claude.com/docs/en/agents-and-tools/agent-skills/overview
- https://github.com/CoderGamester/mcp-unity
- https://github.com/CoplayDev/unity-mcp
- https://github.com/Coding-Solo/godot-mcp
- https://help.figma.com/hc/en-us/articles/32132100833559-Guide-to-the-Figma-MCP-server
- https://www.pulsemcp.com/servers/game-assets (Ludo.ai)
- https://github.com/MubarakHAlketbi/game-asset-mcp
- https://github.com/tadasant/mcp-server-stability-ai
- https://github.com/shinpr/mcp-image
- https://www.pulsemcp.com/servers/vast-ai-tripo-3d
- https://www.pulsemcp.com/servers/fabianh001-threedee-meshy
- https://www.pulsemcp.com/servers/sound-effects
- https://firebase.google.com/docs/ai-assistance/mcp-server
- https://github.com/gannonh/firebase-mcp
- https://docs.mixpanel.com/docs/mcp
- https://github.com/silviorodrigues/amplitude-mcp
- https://developers.google.com/knowledge/mcp
- https://github.com/devexpert-io/play-store-mcp
- https://mcpservers.org/servers/BlocktopusLtd/mcp-google-play
- https://docs.bitrise.io/en/bitrise-platform/ai/bitrise-mcp.html
- https://www.mindstudio.ai/blog/claude-code-agentic-workflow-patterns
- https://claudefa.st/blog/guide/agents/sub-agent-best-practices
