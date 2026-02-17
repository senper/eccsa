import fs from "fs";
import fetch from "node-fetch";
import { create } from "xmlbuilder2";

var pathList = [];
var currentDate = new Date().toISOString();

const lawTypes = ["proclamation", "regulation", "directive"];
const resourceTypes = [
  "books",
  "Publications",
  "Newspapers",
  "External Links",
  "Downloads",
  "Laws and Directives",
  "Other Materials",
];

const solist = [];
await fetch("https://ethiopianchamber.org.et/api/ApiServiceType/owners")
  .then((response) => response.json())
  .then((sos) => {
    sos.$values.forEach((so) => {
      solist.push(so.title.toLowerCase().replaceAll(" ", "").replaceAll("&", "_"));
    });
  });

fetch("https://ethiopianchamber.org.et/api/ApiWebsite/Menus")
  .then((response) => response.json())
  .then(async (json) => {
    await json.$values.map(async (item) => {
      pathList.push(item.path);
      if (item.title.toLowerCase() == "services") {
        solist.forEach((t) => {
          pathList.push(item.path + "/" + t);
        });
      }
      item.children.$values.map((child) => {
        pathList.push(child.path);
        if (item.title.toLowerCase() == "laws") {
          lawTypes.forEach((t) => {
            pathList.push(child.path + "/" + t);
          });
        } else if (
          item.title.toLowerCase() == "resources" &&
          child.path == "/eresources"
        ) {
          resourceTypes.forEach((t) => {
            pathList.push(
              child.path + "/" + t.toLowerCase().replaceAll(" ", "")
            );
          });
        }
      });
    });
    let clearedPath = [...new Set(pathList)];

    // creating the sitemap.xml file
    const root = create({
      version: "1.0",
      encoding: "UTF-8",
    }).ele("urlset", {
      xmlns: "http://www.sitemaps.org/schemas/sitemap/0.9",
      "xmlns:news": "http://www.google.com/schemas/sitemap-news/0.9",
      "xmlns:xhtml": "http://www.w3.org/1999/xhtml",
      "xmlns:mobile": "http://www.google.com/schemas/sitemap-mobile/1.0",
      "xmlns:image": "http://www.google.com/schemas/sitemap-image/1.1",
      "xmlns:video": "http://www.google.com/schemas/sitemap-video/1.1",
    });
    clearedPath.map((item) => {
      const itemLink = root
        .ele("url")
        .ele("loc")
        .txt(`https://ethiopianchamber.et${item}`)
        .up()
        .ele("lastmod")
        .txt(currentDate)
        .up()
        .up()
        .up();
    });
    const xml = root.end({ prettyPrint: true });
    let full_file_name = "./" + "sitemap" + ".xml";
    fs.writeFileSync(full_file_name, xml, function (err) {
      if (err) throw err;
    });
  });
