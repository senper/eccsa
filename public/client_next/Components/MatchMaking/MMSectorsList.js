import { Accordion, AccordionDetails, AccordionSummary } from "@mui/material";
import React from "react";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";

const subSectorUlA = {
  margin: "0px",
  padding: "5px",
  display: "block",
  borderBottom: "solid 1px #eee",
  textDecoration: "none",
  color: "#000000",
  fontSize: "14px",
  cursor: "pointer"
};
const subSectorUlAS = {
  ...subSectorUlA,
  color: "blue",
};
export default function MMSectorsList({
  sectors,
  loadDirectory,
  selectedSector,
  counts,
  sectorId,
  getLocal={getLocal}
}) {
  function getStyle(selected) {
    const st = selected ? subSectorUlAS : subSectorUlA;
    return st;
  }
  return (
    <>
      <h3 align="center">{getLocal("Business sectors")}</h3>
      {sectors
        ? sectors
            .filter((s) => !s.parentId)
            .map((so, index) => (
                <Accordion defaultExpanded={so.id === sectorId} key={index}>
                  <AccordionSummary
                    expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}
                    style={{ backgroundColor: "#f5f5f5" }}
                  >
                    <div className="icon"></div>
                    <br />
                    <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
                      <span style={{ color: "black", fontSize: "14px" }}>
                        {so.name}
                      </span>
                    </div>
                  </AccordionSummary>
                  <AccordionDetails style={{ display: "block" }}>
                    {sectors
                      .filter((s) => s.parentId == so.id)
                      .map((ass, index) => (
                        <a key={index}
                          role="list"
                          title={ass.nameEng}
                          style={getStyle(
                            selectedSector && selectedSector.id == ass.id
                          )}
                          onClick={() => loadDirectory(ass)}
                        >
                          {ass.name} (
                          {counts.find((c) => c.sectorId == ass.id)
                            ? counts.find((c) => c.sectorId == ass.id).count
                            : 0}
                          )
                        </a>
                      ))}
                  </AccordionDetails>
                </Accordion>
            ))
        : null}
    </>
  );
}
