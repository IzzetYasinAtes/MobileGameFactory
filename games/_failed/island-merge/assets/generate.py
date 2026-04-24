"""Brief prompt'larindan karakter + ortam sprite'lari uret (SD-Turbo CPU).
Once model indir + 1 test, sonra batch."""
import os, sys, time, argparse
from pathlib import Path

from diffusers import AutoPipelineForText2Image
import torch

SCRIPT_DIR = Path(__file__).parent
OUT = SCRIPT_DIR
RAW = SCRIPT_DIR / "raw"
RAW.mkdir(exist_ok=True)

# SD-Turbo (SD 2.1 distilled, 1-step inference, ~2.5GB)
MODEL = "stabilityai/sd-turbo"

NEGATIVE = "text, watermark, logo, signature, blurry, cropped, extra limbs, bad anatomy, 3d render, realistic, photo, nsfw, ugly, deformed"

JOBS = {
    "character-kasif": dict(
        seed=42,
        prompt="cute cartoon explorer kid, big eyes, wearing safari hat and brown backpack, yellow shirt, stylized 2D mobile game character, soft warm colors, full body, standing pose, centered, plain neutral background, clean silhouette, children book illustration",
    ),
    "character-lila": dict(
        seed=43,
        prompt="cute cartoon explorer girl, big green eyes, wearing yellow straw hat with flower, brown braids, safari outfit, stylized 2D mobile game character, soft warm colors, full body, standing pose, plain neutral background, clean silhouette, children book illustration",
    ),
    "character-momo": dict(
        seed=44,
        prompt="cute cartoon monkey companion, expressive friendly face, small brown monkey with green tiny backpack, stylized 2D mobile game pet, soft colors, full body, sitting pose, plain neutral background, clean silhouette, children book illustration",
    ),
    "character-papagan": dict(
        seed=45,
        prompt="cute cartoon parrot companion, red blue yellow feathers, big expressive eyes, stylized 2D mobile game pet, soft colors, full body, standing perched, plain neutral background, clean silhouette, children book illustration",
    ),
    "env-forest": dict(
        seed=100,
        prompt="tropical jungle environment with ancient ruins in background, vibrant green plants, waterfalls, stone steps, warm sunlight, stylized 2D mobile game background, fantasy adventure, painterly illustration, no text",
    ),
    "env-sahil": dict(
        seed=101,
        prompt="tropical beach with cave entrance, sailboat in distance, golden sand, palm trees, blue ocean, stylized 2D mobile game background, painterly illustration, warm sunset light, no text",
    ),
    "env-tapinak": dict(
        seed=102,
        prompt="ancient stone temple interior, warm torch lighting, golden door, mysterious atmosphere, stylized 2D mobile game environment, painterly illustration, adventure theme, no text",
    ),
    "env-volkan": dict(
        seed=103,
        prompt="volcanic landscape, flowing lava rivers, dramatic orange red lighting, dark rocks, stylized 2D mobile game art, painterly illustration, no text",
    ),
    "env-buz": dict(
        seed=104,
        prompt="icy frozen world, snow covered ruins, blue cyan tones, icicles, stylized 2D mobile game environment, painterly illustration, magical atmosphere, no text",
    ),
}


def load_pipe():
    print(f"Loading {MODEL} (CPU)…")
    pipe = AutoPipelineForText2Image.from_pretrained(
        MODEL,
        torch_dtype=torch.float32,
        variant=None,
        safety_checker=None,
        requires_safety_checker=False,
    ).to("cpu")
    pipe.set_progress_bar_config(disable=True)
    return pipe


def render(pipe, name, cfg, size=512):
    generator = torch.Generator("cpu").manual_seed(cfg["seed"])
    t0 = time.time()
    image = pipe(
        prompt=cfg["prompt"] + ", stylized cartoon, 2d illustration",
        negative_prompt=NEGATIVE,
        num_inference_steps=4,
        guidance_scale=0.0,   # SD-Turbo prefers cfg=0
        height=size,
        width=size,
        generator=generator,
    ).images[0]
    dur = time.time() - t0
    raw_path = RAW / f"{name}.png"
    image.save(raw_path)
    print(f"  {name}: {dur:.1f}s -> {raw_path}")
    return raw_path


def remove_bg(src_path, dst_path):
    try:
        from rembg import remove
        from PIL import Image
        with open(src_path, "rb") as f:
            data = f.read()
        out = remove(data)
        with open(dst_path, "wb") as f:
            f.write(out)
        print(f"  bg removed -> {dst_path}")
    except Exception as e:
        print(f"  rembg fail ({e}), copy raw")
        import shutil
        shutil.copy(src_path, dst_path)


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--only", help="tek bir job adi uret (test)")
    parser.add_argument("--size", type=int, default=512)
    parser.add_argument("--no-bg", action="store_true", help="rembg atla")
    args = parser.parse_args()

    pipe = load_pipe()

    jobs = {args.only: JOBS[args.only]} if args.only else JOBS
    total_t0 = time.time()
    for name, cfg in jobs.items():
        raw = render(pipe, name, cfg, size=args.size)
        if name.startswith("character-") and not args.no_bg:
            remove_bg(raw, OUT / f"{name}.png")
        else:
            import shutil
            shutil.copy(raw, OUT / f"{name}.png")
    print(f"Total: {time.time()-total_t0:.1f}s")


if __name__ == "__main__":
    main()
