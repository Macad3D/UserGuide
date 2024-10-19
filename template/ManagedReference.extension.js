exports.postTransform = function (model) 
{
  if (!model) return null;

  handleItem(model);

  return model;
}

function handleItem(item) 
{
  if (!item) return null;
  
  if (item.children) 
  {
    item.children.forEach(function (item) 
    {
      handleItem(item);
    });
  }

  // Currently there's nothing to do, max change in the future
}
