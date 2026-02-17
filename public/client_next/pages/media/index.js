import { useRouter } from "next/router";

export default function TextMediaDefault() {
    const location = useRouter();
    location.push("/media/announcement");
}