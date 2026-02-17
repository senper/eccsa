import React from 'react';
import {
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Link,
} from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';

const subSectorUlA = {
  margin: '0px',
  padding: '5px',
  display: 'block',
  borderBottom: 'solid 1px #eee',
  textDecoration: 'none',
  color: '#000000',
  fontSize: '14px',
};
const subSectorUlAS = {
  ...subSectorUlA,
  color: 'blue',
};
export default function SectorsList({
  sectors,
  loadDirectory,
  selectedSector,
  counts,
}) {
  function getStyle(selected) {
    const st = selected ? subSectorUlAS : subSectorUlA;
    return st;
  }
  return (
    <>
      <h3 align="center">Business sectors</h3>
      {sectors
        ? sectors
            .filter((s) => !s.parentId)
            .map((so, index) => (
              <Accordion key={index}>
                <AccordionSummary
                  expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}
                  style={{ backgroundColor: '#f5f5f5' }}
                >
                  <div className="icon"></div>
                  <br />
                  <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
                    <span style={{ color: 'black', fontSize: '14px' }}>
                      {so.name}
                    </span>
                  </div>
                </AccordionSummary>
                <AccordionDetails style={{ display: 'block' }}>
                  {sectors
                    .filter((s) => s.parentId == so.id)
                    .map((ass, index) => (
                      <Link
                        role="list"
                        title={ass.nameEng}
                        style={getStyle(
                          selectedSector && selectedSector.id == ass.id
                        )}
                        onClick={() => loadDirectory(ass)}
                        key={index}
                      >
                        {ass.name} (
                        {counts.find((c) => c.sectorId == ass.id)
                          ? counts.find((c) => c.sectorId == ass.id).count
                          : 0}
                        )
                      </Link>
                    ))}
                </AccordionDetails>
              </Accordion>
            ))
        : null}
    </>
  );
}
