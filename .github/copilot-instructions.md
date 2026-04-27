# Copilot Instructions

## Project Guidelines
- In the Ember project, content management follows a versioning system instead of traditional update/delete operations. When updating content, a new version is created with IsActive=true, and the previous version is marked IsActive=false with a RemovedTime. "Deletion" sets IsActive=false to preserve history. Always use versioning for content modifications, never destructive updates or deletes.
- Identifier is the feild that we use to find contetn in the system, it is unique and never change, even when content is updated. It is used to find content in the system, and it is also used to link content together. For example, a product may have a list of reviews, and the reviews are linked to the product using the product's identifier.