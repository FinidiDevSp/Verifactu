import re
from difflib import SequenceMatcher


def normalize_mix_name(title: str) -> str:
    """Remove content in parentheses or brackets, often mix names."""
    return re.sub(r'\s*[\(\[].*?[\)\]]', '', title).strip()


def compare_titles(t_vk: str, t_down: str, threshold: float = 0.9) -> bool:
    """Compare titles, ignoring mix name if initial match is below threshold."""
    ratio = SequenceMatcher(None, t_vk, t_down).ratio()
    if ratio >= threshold:
        return True
    t_down_no_mix = normalize_mix_name(t_down)
    ratio_no_mix = SequenceMatcher(None, t_vk, t_down_no_mix).ratio()
    return ratio_no_mix >= threshold


if __name__ == "__main__":
    vk_title = "Artist - Track"
    downloaded_title = "Artist - Track (Extended Mix)"
    print(compare_titles(vk_title, downloaded_title))
