# Galleria - Prompt Rules

## Rule #1: Root Scan Folders
The following ROOT folders are scanned for image content:
- I:\Album
- I:\Category
- I:\Highlight
- I:\Person
- I:\Studio

## Rule #2: Tag Hierarchy
Tags are derived from folder names based on the following prefix hierarchy:

1. Person: folders starting with `p-`
2. Album: folders starting with `a-`
3. Studio: folders starting with `s-`
4. Type: folders starting with `t-`
5. Gallery: folders starting with `g-`
6. Category: folders starting with `c-`
7. Highlight: folders starting with `h-`

## Rule #3: Breadcrumb construction
1. Use the Rule #2 Heirarchy
2. Remove the 2 character tag which should leave a string of more than 1 character - this is the stripped-tag
3. Assemble the full breadcrumb from the tags
4. When there are more than 1 of a tag type, store them in alphanumeric order
5. Final breadcrumb string should be in the form of "stripped-tag | stripped-tag | stripped-tag | stripped-tag"

## Rule #4: Breadcrumb Exception
1. If there is more than on p- tag for a file, create a new breadcrumb for each p- person.