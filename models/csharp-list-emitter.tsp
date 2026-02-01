import "@typespec/compiler";
import "@typespec/http";
import "@typespec/openapi";
import "@typespec/compiler/emitter-csharp";

using Cadl;

@@extension
namespace CSharpListOverride {

  model CSharpListOverride extends EmitterCSharp.Evaluator {
    
    // Override array handling
    override emitArrayType(target, elementType, visibility) {
      const emittedElement = this.emitType(elementType);
      return `List<${emittedElement}>`;
    }
  }
}
