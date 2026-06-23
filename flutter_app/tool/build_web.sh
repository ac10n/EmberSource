#!/usr/bin/env bash
set -euo pipefail

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
repo_dir="$(cd "${script_dir}/.." && pwd)"

cd "${repo_dir}"

build_id="${BUILD_ID:-$(git rev-parse --short HEAD 2>/dev/null || true)}"
if [[ -z "${build_id}" ]]; then
  build_id="$(date -u +%Y%m%d%H%M%S)"
else
  build_id="${build_id}-$(date -u +%Y%m%d%H%M%S)"
fi

flutter build web --release

python3 - "$build_id" <<'PY'
from pathlib import Path
import sys

build_id = sys.argv[1]
web_dir = Path.cwd() / 'build' / 'web'

index_html = web_dir / 'index.html'
bootstrap_js = web_dir / 'flutter_bootstrap.js'

index_text = index_html.read_text()
index_text = index_text.replace(
    '<script src="flutter_bootstrap.js" async></script>',
    f'<script src="flutter_bootstrap.js?v={build_id}" async></script>',
)
index_html.write_text(index_text)

bootstrap_text = bootstrap_js.read_text()
bootstrap_text = bootstrap_text.replace(
    '"mainJsPath":"main.dart.js"',
    f'"mainJsPath":"main.dart.js?v={build_id}"',
)
bootstrap_js.write_text(bootstrap_text)
PY