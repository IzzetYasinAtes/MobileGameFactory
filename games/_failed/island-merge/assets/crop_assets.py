"""Brand keyart'tan temizlenmis sprite'lar uret.
1179x796 referans image'dan karakter + ortam crop'lari + bg temizligi."""
from PIL import Image
from pathlib import Path

src = Image.open("brand-keyart.png").convert("RGBA")
W, H = src.size
print(f"source: {W}x{H}")

out = Path(".")

# KARAKTERLER paneli — 4 portre yan yana
# etiket alti kesilir, karakter izole
char_row_left = 315
char_row_right = 760
char_top = 35
char_bottom = 240   # etiket yukarisinda kes
char_panel_w = char_row_right - char_row_left
per = char_panel_w // 4  # ~111

names = ["kasif", "lila", "momo", "papagan"]
# panel icinde her portre genisligi per; hafif margin ile crop
margin_x = 4
for i, name in enumerate(names):
    left = char_row_left + i * per + margin_x
    right = char_row_left + (i + 1) * per - margin_x
    portrait = src.crop((left, char_top, right, char_bottom))
    portrait.save(out / f"character-{name}.png")

# Genel karakter strip (menu banner icin)
src.crop((char_row_left, char_top, char_row_right, char_bottom + 30)).save(out / "characters-strip.png")

# ORTAMLAR paneli
# Label satirini atla, her gorselin altindaki etiketi atla

# Buyuk orman
src.crop((300, 298, 770, 505)).save(out / "env-forest.png")
# Alt sira 4 kucuk — label'lari dahil etmemek icin dikkatli box
env_coords = {
    "sahil": (300, 515, 528, 640),     # sahil magarasi kutusu
    "tapinak": (534, 515, 770, 640),   # antik tapinak
    "volkan": (300, 650, 528, 775),    # volkanik bolge
    "buz": (534, 650, 770, 775),       # buz diyari
}
for name, box in env_coords.items():
    img = src.crop(box)
    # Alt kisimdaki etiket banner'ini kes (son ~25px)
    w, h = img.size
    img = img.crop((0, 0, w, h - 28))
    img.save(out / f"env-{name}.png")

# Logo (Mini Kasifler)
src.crop((15, 15, 280, 150)).save(out / "logo.png")

# Karakter kostum varyasyonlari (sag alt)
src.crop((805, 660, 1175, 790)).save(out / "character-costumes.png")

# Kucuk board icon'lari icin icon sheet (opsiyonel — MAUI kendi SkiaSharp ile cizecek)
print("done")
