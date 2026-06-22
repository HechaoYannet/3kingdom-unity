#!/usr/bin/env bash
# ============================================================
# setup.sh — one-time hook installer for new clones
#
# Usage:
#   bash .githooks/setup.sh
#
# Or run it automatically:
#   Add to your shell profile: cd project && bash .githooks/setup.sh
# ============================================================

git config core.hooksPath .githooks
echo "✓ Git hooksPath 已设置为: .githooks/"
echo ""
echo "Hooks 清单:"
echo "  commit-msg  — 强制 commit 消息格式 (commit 时触发)"
echo "  pre-commit  — 暂存文件检查 (commit 前触发)"
echo "  pre-push    — 推送完整性校验 (push 前触发)"
echo ""
echo "跳过检查:"
echo "  SKIP_PRECOMMIT_HOOK=1 git commit -m '...'"
echo "  SKIP_PREPUSH_HOOK=1 git push"
