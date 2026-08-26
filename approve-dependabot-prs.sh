#!/bin/bash

# Script to approve all open dependabot PRs in order from oldest to newest
# Usage: ./approve-dependabot-prs.sh

# Array of PR numbers in order from oldest to newest
PR_NUMBERS=(
  106 108 109 112 113 114 118 126 127 128 131 133 134 135 136 139 140
  142 143 144 145 146 148 149 152 153 156 157 158 159 160 161 162 163
  164 165 166 167 168 169
)

echo "Starting to approve dependabot PRs..."
echo "Total PRs to approve: ${#PR_NUMBERS[@]}"
echo ""

approved_count=0
failed_count=0

for pr_number in "${PR_NUMBERS[@]}"; do
  echo "Approving PR #$pr_number..."
  
  if gh pr review "$pr_number" --approve; then
    echo "✓ PR #$pr_number approved successfully"
    ((approved_count++))
  else
    echo "✗ Failed to approve PR #$pr_number"
    ((failed_count++))
  fi
  
  echo ""
done

echo "=========================================="
echo "Approval Summary:"
echo "✓ Successfully approved: $approved_count"
echo "✗ Failed: $failed_count"
echo "=========================================="
