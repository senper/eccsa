import { Breadcrumbs } from "@mui/material";
import Link from "next/link";
import { getLocalByKey } from "../../pages/api/menu";

export const CommonBreadcrumb = ({ links = [] }) => {
  return <>
    <Breadcrumbs aria-label="breadcrumb">
      {links.map((l, index) => (
        <Link key={index} href={l.path} legacyBehavior>
          {l.title}
        </Link>
      ))}
    </Breadcrumbs>
  </>;
};
